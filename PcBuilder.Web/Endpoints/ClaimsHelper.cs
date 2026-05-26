using System.Security.Claims;

namespace PcBuilder.Web.Endpoints;

public static class ClaimsHelper
{
    public static long ObtenerUsuarioId(this ClaimsPrincipal user)
    {
        var claim = user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("Token inválido.");

        return long.Parse(claim);
    }

    public static bool EsAdministrador(this ClaimsPrincipal user) =>
        user.IsInRole("Administrador");
}
