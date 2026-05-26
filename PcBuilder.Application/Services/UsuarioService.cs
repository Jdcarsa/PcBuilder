using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Exceptions;
using PcBuilder.Domain.Interfaces;
using PcBuilder.Infrastructure.Services;

namespace PcBuilder.Application.Services;

public class UsuarioService(IUsuarioRepository usuarioRepo, IUnitOfWork uow, IPasswordHasher passwordHasher,
    IJwtService jwtService) : IUsuarioService
{
    public async Task<UsuarioResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default)
    {
        var usuario = await usuarioRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Usuario", id);

        return usuario.ToResponse();
    }

    public async Task<UsuarioResponse> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default)
    {
        if (await usuarioRepo.ExisteEmailAsync(request.Email, ct))
            throw new EmailDuplicadoException(request.Email);

        var hash = passwordHasher.Hash(request.Password);
        var usuario = Usuario.Crear(request.Nombre, request.Apellido, request.Email, hash);

        await usuarioRepo.AgregarAsync(usuario, ct);
        await uow.GuardarCambiosAsync(ct);

        return usuario.ToResponse();
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var usuario = await usuarioRepo.ObtenerPorEmailAsync(request.Email, ct)
            ?? throw new UnauthorizedAccessException("Credenciales inválidas.");

        if (!usuario.EstaActivo)
            throw new UnauthorizedAccessException("La cuenta está desactivada.");

        if (!passwordHasher.Verificar(request.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        var token = jwtService.GenerarToken(usuario);
        return new LoginResponse(token, usuario.ToResponse());
    }

    public async Task ActualizarPerfilAsync(long id, ActualizarPerfilRequest request, CancellationToken ct = default)
    {
        var usuario = await usuarioRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Usuario", id);

        usuario.ActualizarPerfil(request.Nombre, request.Apellido);
        usuarioRepo.Actualizar(usuario);
        await uow.GuardarCambiosAsync(ct);
    }

    public async Task CambiarPasswordAsync(long id, CambiarPasswordRequest request, CancellationToken ct = default)
    {
        var usuario = await usuarioRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Usuario", id);

        if (!passwordHasher.Verificar(request.PasswordActual, usuario.PasswordHash))
            throw new UnauthorizedAccessException("La contraseña actual es incorrecta.");

        var nuevoHash = passwordHasher.Hash(request.NuevoPassword);
        usuario.CambiarPassword(nuevoHash);
        usuarioRepo.Actualizar(usuario);
        await uow.GuardarCambiosAsync(ct);
    }
}
