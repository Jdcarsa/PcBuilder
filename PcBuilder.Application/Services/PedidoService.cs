using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Exceptions;
using PcBuilder.Domain.Interfaces;

namespace PcBuilder.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepo,
    IComponenteRepository componenteRepo,
    IConfiguracionPCRepository configuracionRepo,
    IUnitOfWork uow) : IPedidoService
{
    public async Task<List<PedidoResponse>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default)
    {
        var pedidos = await pedidoRepo.ObtenerPorUsuarioAsync(usuarioId, ct);
        return pedidos.Select(p => p.ToResponse()).ToList();
    }

    public async Task<PedidoResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", id);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> CrearAsync(long usuarioId, CrearPedidoRequest request, CancellationToken ct = default)
    {
        var pedido = Pedido.Crear(usuarioId, request.DireccionEnvio, request.NotasCliente);

        if (request.ConfiguracionId.HasValue)
        {
            var configuracion = await configuracionRepo.ObtenerPorIdAsync(request.ConfiguracionId.Value, ct)
                ?? throw new EntidadNoEncontradaException("ConfiguracionPC", request.ConfiguracionId.Value);

            pedido.AgregarDesdeConfiguracion(configuracion);
        }

        await pedidoRepo.AgregarAsync(pedido, ct);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> AgregarItemAsync(long pedidoId, AgregarItemPedidoRequest request, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", pedidoId);

        var componente = await componenteRepo.ObtenerPorIdAsync(request.ComponenteId, ct)
            ?? throw new EntidadNoEncontradaException("Componente", request.ComponenteId);

        pedido.AgregarItem(componente, request.Cantidad);
        pedidoRepo.Actualizar(pedido);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> ConfirmarAsync(long pedidoId, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", pedidoId);

        pedido.Confirmar();

        foreach (var item in pedido.Items)
        {
            var componente = await componenteRepo.ObtenerPorIdAsync(item.ComponenteId, ct)
                ?? throw new EntidadNoEncontradaException("Componente", item.ComponenteId);

            componente.DescontarStock(item.Cantidad);
            componenteRepo.Actualizar(componente);
        }

        pedidoRepo.Actualizar(pedido);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> IniciarProcesamientoAsync(long pedidoId, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", pedidoId);

        pedido.IniciarProcesamiento();
        pedidoRepo.Actualizar(pedido);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> EnviarAsync(long pedidoId, EnviarPedidoRequest request, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", pedidoId);

        pedido.Enviar(request.NumeroGuia);
        pedidoRepo.Actualizar(pedido);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> MarcarEntregadoAsync(long pedidoId, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", pedidoId);

        pedido.MarcarEntregado();
        pedidoRepo.Actualizar(pedido);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }

    public async Task<PedidoResponse> CancelarAsync(long pedidoId, CancellationToken ct = default)
    {
        var pedido = await pedidoRepo.ObtenerPorIdAsync(pedidoId, ct)
            ?? throw new EntidadNoEncontradaException("Pedido", pedidoId);

        pedido.Cancelar();
        pedidoRepo.Actualizar(pedido);
        await uow.GuardarCambiosAsync(ct);

        return pedido.ToResponse();
    }
}
