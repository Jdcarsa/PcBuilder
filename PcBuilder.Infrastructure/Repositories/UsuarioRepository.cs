using Microsoft.EntityFrameworkCore;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Interfaces;
using PcBuilder.Infrastructure.Data;

namespace PcBuilder.Infrastructure.Repositories;

public class UsuarioRepository(AppDbContext db) : IUsuarioRepository
{
    public async Task<Usuario?> ObtenerPorIdAsync(long id, CancellationToken ct = default) =>
        await db.Usuarios.FindAsync([id], ct);

    public async Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default) =>
        await db.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);

    public async Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default) =>
        await db.Usuarios
            .AnyAsync(u => u.Email == email.ToLowerInvariant().Trim(), ct);

    public async Task AgregarAsync(Usuario usuario, CancellationToken ct = default) =>
        await db.Usuarios.AddAsync(usuario, ct);

    public void Actualizar(Usuario usuario) =>
        db.Usuarios.Update(usuario);
}
