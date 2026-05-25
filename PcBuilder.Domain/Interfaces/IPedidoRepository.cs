using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Enums;

namespace PcBuilder.Domain.Interfaces;

public interface IPedidoRepository
{
    Task<Pedido?> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<Pedido?> ObtenerPorNumeroAsync(string numeroPedido, CancellationToken ct = default);
    Task<List<Pedido>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default);
    Task<List<Pedido>> ObtenerPorEstadoAsync(EstadoPedido estado, CancellationToken ct = default);
    Task AgregarAsync(Pedido pedido, CancellationToken ct = default);
    void Actualizar(Pedido pedido);
}