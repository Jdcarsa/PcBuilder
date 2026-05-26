using System.Security.Claims;
using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;

namespace PcBuilder.Web.Endpoints;

public static class UsuarioEndpoints
{
    public static RouteGroupBuilder MapUsuarioEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/me", async (ClaimsPrincipal user, IUsuarioService service, CancellationToken ct) =>
        {
            var id = user.ObtenerUsuarioId();
            var usuario = await service.ObtenerPorIdAsync(id, ct);
            return Results.Ok(usuario);
        })
        .WithName("GetPerfil")
        .WithSummary("Obtener perfil del usuario autenticado")
        .RequireAuthorization();

        group.MapPut("/me", async (ActualizarPerfilRequest request, ClaimsPrincipal user, IUsuarioService service,
            CancellationToken ct) =>
        {
            var id = user.ObtenerUsuarioId();
            await service.ActualizarPerfilAsync(id, request, ct);
            return Results.NoContent();
        })
        .WithName("ActualizarPerfil")
        .WithSummary("Actualizar perfil del usuario autenticado")
        .RequireAuthorization();

        group.MapPatch("/me/password", async (CambiarPasswordRequest request, ClaimsPrincipal user, IUsuarioService service,
            CancellationToken ct) =>
        {
            var id = user.ObtenerUsuarioId();
            await service.CambiarPasswordAsync(id, request, ct);
            return Results.NoContent();
        })
        .WithName("CambiarPassword")
        .WithSummary("Cambiar contraseña del usuario autenticado")
        .RequireAuthorization();

        return group;
    }
}
