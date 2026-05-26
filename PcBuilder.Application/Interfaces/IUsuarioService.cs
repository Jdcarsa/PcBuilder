using PcBuilder.Application.DTOs;

namespace PcBuilder.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<UsuarioResponse> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task ActualizarPerfilAsync(long id, ActualizarPerfilRequest request, CancellationToken ct = default);
    Task CambiarPasswordAsync(long id, CambiarPasswordRequest request, CancellationToken ct = default);
}