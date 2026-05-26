using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;

namespace PcBuilder.Web.Endpoints;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(this RouteGroupBuilder group)
    {
        group.MapPost("/registro",
                async (RegistrarUsuarioRequest request, IUsuarioService service, CancellationToken ct) =>
                {
                    var usuario = await service.RegistrarAsync(request, ct);
                    return Results.Created($"/api/usuarios/{usuario.Id}", usuario);
                })
            .WithName("Registro")
            .WithSummary("Registrar nuevo usuario")
            .AllowAnonymous();

        group.MapPost("/login", async (LoginRequest request, IUsuarioService service, CancellationToken ct) =>
            {
                var resultado = await service.LoginAsync(request, ct);
                return Results.Ok(resultado);
            })
            .WithName("Login")
            .WithSummary("Iniciar sesión")
            .AllowAnonymous();

        return group;
    }
}