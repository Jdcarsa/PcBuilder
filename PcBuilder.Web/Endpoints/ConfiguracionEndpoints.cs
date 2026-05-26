using System.Security.Claims;
using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;

namespace PcBuilder.Web.Endpoints;

public static class ConfiguracionEndpoints
{
    public static RouteGroupBuilder MapConfiguracionEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (ClaimsPrincipal user, IConfiguracionPCService service, CancellationToken ct) =>
            {
                var usuarioId = user.ObtenerUsuarioId();
                var configuraciones = await service.ObtenerPorUsuarioAsync(usuarioId, ct);
                return Results.Ok(configuraciones);
            })
            .WithName("GetConfiguraciones")
            .WithSummary("Listar configuraciones del usuario")
            .RequireAuthorization();

        group.MapGet("/{id:long}", async (long id, IConfiguracionPCService service, CancellationToken ct) =>
            {
                var configuracion = await service.ObtenerPorIdAsync(id, ct);
                return Results.Ok(configuracion);
            })
            .WithName("GetConfiguracion")
            .WithSummary("Obtener configuración por Id")
            .RequireAuthorization();

        group.MapPost("/", async (CrearConfiguracionRequest request, ClaimsPrincipal user,
                IConfiguracionPCService service,
                CancellationToken ct) =>
            {
                var usuarioId = user.ObtenerUsuarioId();
                var configuracion = await service.CrearAsync(usuarioId, request, ct);
                return Results.Created($"/api/configuraciones/{configuracion.Id}", configuracion);
            })
            .WithName("CrearConfiguracion")
            .WithSummary("Crear nueva configuración de PC")
            .RequireAuthorization();

        group.MapPost("/{id:long}/componentes", async (long id, AgregarComponenteRequest request,
                IConfiguracionPCService service,
                CancellationToken ct) =>
            {
                var configuracion = await service.AgregarComponenteAsync(id, request, ct);
                return Results.Ok(configuracion);
            })
            .WithName("AgregarComponente")
            .WithSummary("Agregar componente a la configuración")
            .RequireAuthorization();

        group.MapDelete("/{id:long}/componentes/{componenteId:long}", async (long id, long componenteId,
                IConfiguracionPCService service, CancellationToken ct) =>
            {
                var configuracion = await service.RemoverComponenteAsync(id, componenteId, ct);
                return Results.Ok(configuracion);
            })
            .WithName("RemoverComponente")
            .WithSummary("Remover componente de la configuración")
            .RequireAuthorization();

        group.MapGet("/{id:long}/validar", async (long id, IConfiguracionPCService service, CancellationToken ct) =>
            {
                var resultado = await service.ValidarCompatibilidadAsync(id, ct);
                return Results.Ok(resultado);
            })
            .WithName("ValidarConfiguracion")
            .WithSummary("Validar compatibilidad de la configuración")
            .RequireAuthorization();

        group.MapPost("/{id:long}/finalizar", async (long id, IConfiguracionPCService service, CancellationToken ct) =>
            {
                var configuracion = await service.FinalizarAsync(id, ct);
                return Results.Ok(configuracion);
            })
            .WithName("FinalizarConfiguracion")
            .WithSummary("Finalizar configuración de PC")
            .RequireAuthorization();

        group.MapDelete("/{id:long}", async (long id, IConfiguracionPCService service, CancellationToken ct) =>
            {
                await service.EliminarAsync(id, ct);
                return Results.NoContent();
            })
            .WithName("EliminarConfiguracion")
            .WithSummary("Eliminar configuración")
            .RequireAuthorization();

        return group;
    }
}