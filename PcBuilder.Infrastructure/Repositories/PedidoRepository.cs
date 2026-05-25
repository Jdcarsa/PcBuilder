using Microsoft.EntityFrameworkCore;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Enums;
using PcBuilder.Domain.Interfaces;
using PcBuilder.Infrastructure.Data;

namespace PcBuilder.Infrastructure.Repositories;

public class PedidoRepository(AppDbContext db) : IPedidoRepository
{
    public async Task<Pedido?> ObtenerPorIdAsync(long id, CancellationToken ct = default) =>
        await db.Pedidos
            .Include(p => p.Items)
                .ThenInclude(i => i.Componente)
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    public async Task<Pedido?> ObtenerPorNumeroAsync(string numeroPedido, CancellationToken ct = default) =>
        await db.Pedidos
            .Include(p => p.Items)
                .ThenInclude(i => i.Componente)
            .FirstOrDefaultAsync(p => p.NumeroPedido == numeroPedido, ct);

    public async Task<List<Pedido>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default) =>
        await db.Pedidos
            .Include(p => p.Items)
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.CreadoEn)
            .ToListAsync(ct);

    public async Task<List<Pedido>> ObtenerPorEstadoAsync(EstadoPedido estado, CancellationToken ct = default) =>
        await db.Pedidos
            .Include(p => p.Items)
                .ThenInclude(i => i.Componente)
            .Where(p => p.Estado == estado)
            .OrderByDescending(p => p.CreadoEn)
            .ToListAsync(ct);

    public async Task AgregarAsync(Pedido pedido, CancellationToken ct = default) =>
        await db.Pedidos.AddAsync(pedido, ct);

    public void Actualizar(Pedido pedido) =>
        db.Pedidos.Update(pedido);
}
