using PcBuilder.Domain.Entities;

namespace PcBuilder.Domain.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);
    Task AgregarAsync(Usuario usuario, CancellationToken ct = default);
    void Actualizar(Usuario usuario);
}