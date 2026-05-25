using Microsoft.EntityFrameworkCore;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Interfaces;
using PcBuilder.Infrastructure.Data;

namespace PcBuilder.Infrastructure.Repositories;

public class ConfiguracionPCRepository(AppDbContext db) : IConfiguracionPCRepository
{
    public async Task<ConfiguracionPC?> ObtenerPorIdAsync(long id, CancellationToken ct = default) =>
        await db.ConfiguracionesPC
            .Include(c => c.Items)
                .ThenInclude(i => i.Componente)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

    public async Task<List<ConfiguracionPC>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default) =>
        await db.ConfiguracionesPC
            .Include(c => c.Items)
                .ThenInclude(i => i.Componente)
            .Where(c => c.UsuarioId == usuarioId)
            .OrderByDescending(c => c.CreadoEn)
            .ToListAsync(ct);

    public async Task AgregarAsync(ConfiguracionPC configuracion, CancellationToken ct = default) =>
        await db.ConfiguracionesPC.AddAsync(configuracion, ct);

    public void Actualizar(ConfiguracionPC configuracion) =>
        db.ConfiguracionesPC.Update(configuracion);

    public void Eliminar(ConfiguracionPC configuracion) =>
        db.ConfiguracionesPC.Remove(configuracion);
}
