using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Exceptions;
using PcBuilder.Domain.Interfaces;

namespace PcBuilder.Application.Services;

public class ConfiguracionPCService(
    IConfiguracionPCRepository configuracionRepo,
    IComponenteRepository componenteRepo,
    IUnitOfWork uow) : IConfiguracionPCService
{
    public async Task<List<ConfiguracionPCResponse>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default)
    {
        var configuraciones = await configuracionRepo.ObtenerPorUsuarioAsync(usuarioId, ct);
        return configuraciones.Select(c => c.ToResponse()).ToList();
    }

    public async Task<ConfiguracionPCResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default)
    {
        var configuracion = await configuracionRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("ConfiguracionPC", id);

        return configuracion.ToResponse();
    }

    public async Task<ConfiguracionPCResponse> CrearAsync(long usuarioId, CrearConfiguracionRequest request, CancellationToken ct = default)
    {
        var configuracion = ConfiguracionPC.Crear(usuarioId, request.Nombre);
        await configuracionRepo.AgregarAsync(configuracion, ct);
        await uow.GuardarCambiosAsync(ct);
        return configuracion.ToResponse();
    }

    public async Task<ConfiguracionPCResponse> AgregarComponenteAsync(long configuracionId, AgregarComponenteRequest request, CancellationToken ct = default)
    {
        var configuracion = await configuracionRepo.ObtenerPorIdAsync(configuracionId, ct)
            ?? throw new EntidadNoEncontradaException("ConfiguracionPC", configuracionId);

        var componente = await componenteRepo.ObtenerPorIdAsync(request.ComponenteId, ct)
            ?? throw new EntidadNoEncontradaException("Componente", request.ComponenteId);

        configuracion.AgregarComponente(componente, request.Cantidad);
        configuracionRepo.Actualizar(configuracion);
        await uow.GuardarCambiosAsync(ct);

        return configuracion.ToResponse();
    }

    public async Task<ConfiguracionPCResponse> RemoverComponenteAsync(long configuracionId, long componenteId, CancellationToken ct = default)
    {
        var configuracion = await configuracionRepo.ObtenerPorIdAsync(configuracionId, ct)
            ?? throw new EntidadNoEncontradaException("ConfiguracionPC", configuracionId);

        configuracion.RemoverComponente(componenteId);
        configuracionRepo.Actualizar(configuracion);
        await uow.GuardarCambiosAsync(ct);

        return configuracion.ToResponse();
    }

    public async Task<ValidacionResponse> ValidarCompatibilidadAsync(long configuracionId, CancellationToken ct = default)
    {
        var configuracion = await configuracionRepo.ObtenerPorIdAsync(configuracionId, ct)
            ?? throw new EntidadNoEncontradaException("ConfiguracionPC", configuracionId);

        var resultado = configuracion.ValidarCompatibilidad();
        return new ValidacionResponse(resultado.EsValida, resultado.Errores);
    }

    public async Task<ConfiguracionPCResponse> FinalizarAsync(long configuracionId, CancellationToken ct = default)
    {
        var configuracion = await configuracionRepo.ObtenerPorIdAsync(configuracionId, ct)
            ?? throw new EntidadNoEncontradaException("ConfiguracionPC", configuracionId);

        configuracion.Finalizar();
        configuracionRepo.Actualizar(configuracion);
        await uow.GuardarCambiosAsync(ct);

        return configuracion.ToResponse();
    }

    public async Task EliminarAsync(long configuracionId, CancellationToken ct = default)
    {
        var configuracion = await configuracionRepo.ObtenerPorIdAsync(configuracionId, ct)
            ?? throw new EntidadNoEncontradaException("ConfiguracionPC", configuracionId);

        if (configuracion.EstaFinalizada)
            throw new InvalidOperationException("No se puede eliminar una configuración finalizada.");

        configuracionRepo.Eliminar(configuracion);
        await uow.GuardarCambiosAsync(ct);
    }
}
