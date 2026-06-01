using PcBuilder.Application.DTOs;
using PcBuilder.Domain.Entities;

namespace PcBuilder.Application.Interfaces;

public interface IPedidoService
{
    Task<List<PedidoResponse>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<List<PedidoResponse>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default);
    Task<PedidoResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<PedidoResponse> CrearAsync(long usuarioId, CrearPedidoRequest request, CancellationToken ct = default);
    Task<PedidoResponse> AgregarItemAsync(long pedidoId, AgregarItemPedidoRequest request, CancellationToken ct = default);
    Task<PedidoResponse> ConfirmarAsync(long pedidoId, CancellationToken ct = default);
    Task<PedidoResponse> IniciarProcesamientoAsync(long pedidoId, CancellationToken ct = default);
    Task<PedidoResponse> EnviarAsync(long pedidoId, EnviarPedidoRequest request, CancellationToken ct = default);
    Task<PedidoResponse> MarcarEntregadoAsync(long pedidoId, CancellationToken ct = default);
    Task<PedidoResponse> CancelarAsync(long pedidoId, CancellationToken ct = default);
}