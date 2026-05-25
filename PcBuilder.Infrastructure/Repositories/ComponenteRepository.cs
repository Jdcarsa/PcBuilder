using Microsoft.EntityFrameworkCore;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Enums;
using PcBuilder.Domain.Interfaces;
using PcBuilder.Infrastructure.Data;

namespace PcBuilder.Infrastructure.Repositories;

public class ComponenteRepository(AppDbContext db) : IComponenteRepository
{
    public async Task<Componente?> ObtenerPorIdAsync(long id, CancellationToken ct = default) =>
        await db.Componentes.FindAsync([id], ct);

    public async Task<List<Componente>> ObtenerTodosAsync(CancellationToken ct = default) =>
        await db.Componentes
            .Where(c => c.EstaActivo)
            .OrderBy(c => c.Categoria)
            .ThenBy(c => c.Nombre)
            .ToListAsync(ct);

    public async Task<List<Componente>> ObtenerPorCategoriaAsync(CategoriaComponente categoria, CancellationToken ct = default) =>
        await db.Componentes
            .Where(c => c.Categoria == categoria && c.EstaActivo)
            .OrderBy(c => c.Precio)
            .ToListAsync(ct);

    public async Task<List<Componente>> BuscarAsync(string termino, CancellationToken ct = default)
    {
        var t = termino.ToLower().Trim();
        return await db.Componentes
            .Where(c => c.EstaActivo &&
                        (c.Nombre.ToLower().Contains(t) ||
                         c.Marca.ToLower().Contains(t) ||
                         c.Modelo.ToLower().Contains(t) ||
                         c.Sku.ToLower().Contains(t)))
            .OrderBy(c => c.Nombre)
            .ToListAsync(ct);
    }

    public async Task AgregarAsync(Componente componente, CancellationToken ct = default) =>
        await db.Componentes.AddAsync(componente, ct);

    public void Actualizar(Componente componente) =>
        db.Componentes.Update(componente);

    public void Eliminar(Componente componente) =>
        db.Componentes.Remove(componente);
}
