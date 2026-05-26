namespace PcBuilder.Application.DTOs;

public record UsuarioResponse(
    long Id,
    string Nombre,
    string Apellido,
    string Email,
    string NombreCompleto,
    string Rol,
    bool EstaActivo);

public record RegistrarUsuarioRequest(
    string Nombre,
    string Apellido,
    string Email,
    string Password);

public record LoginRequest(
    string Email,
    string Password);

public record LoginResponse(
    string Token,
    UsuarioResponse Usuario);

public record ActualizarPerfilRequest(
    string Nombre,
    string Apellido);

public record CambiarPasswordRequest(
    string PasswordActual,
    string NuevoPassword);
