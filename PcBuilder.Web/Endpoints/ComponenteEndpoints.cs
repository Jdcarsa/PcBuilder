using Microsoft.AspNetCore.Mvc;
using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;
using PcBuilder.Domain.Enums;

namespace PcBuilder.Web.Endpoints;

public static class ComponenteEndpoints
{
    public static RouteGroupBuilder MapComponenteEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async ([FromQuery] CategoriaComponente? categoria, [FromQuery] string? buscar,
                IComponenteService service, CancellationToken ct) =>
            {
                if (!string.IsNullOrWhiteSpace(buscar))
                    return Results.Ok(await service.BuscarAsync(buscar, ct));

                if (categoria.HasValue)
                    return Results.Ok(await service.ObtenerPorCategoriaAsync(categoria.Value, ct));

                return Results.Ok(await service.ObtenerTodosAsync(ct));
            })
            .WithName("GetComponentes")
            .WithSummary("Listar componentes con filtros opcionales")
            .AllowAnonymous();

        group.MapGet("/{id:long}", async (long id, IComponenteService service, CancellationToken ct) =>
            {
                var componente = await service.ObtenerPorIdAsync(id, ct);
                return Results.Ok(componente);
            })
            .WithName("GetComponente")
            .WithSummary("Obtener componente por Id")
            .AllowAnonymous();

        group.MapPost("/", async (CrearComponenteRequest request, IComponenteService service, CancellationToken ct) =>
            {
                var componente = await service.CrearAsync(request, ct);
                return Results.Created($"/api/componentes/{componente.Id}", componente);
            })
            .WithName("CrearComponente")
            .WithSummary("Crear componente (Admin)")
            .RequireAuthorization("SoloAdmin");

        group.MapPatch("/{id:long}/precio", async (long id, ActualizarPrecioRequest request, IComponenteService service,
                CancellationToken ct) =>
            {
                await service.ActualizarPrecioAsync(id, request, ct);
                return Results.NoContent();
            })
            .WithName("ActualizarPrecio")
            .WithSummary("Actualizar precio (Admin)")
            .RequireAuthorization("SoloAdmin");

        group.MapPatch("/{id:long}/stock", async (long id, AjustarStockRequest request,
                IComponenteService service, CancellationToken ct) =>
            {
                await service.AjustarStockAsync(id, request, ct);
                return Results.NoContent();
            })
            .WithName("AjustarStock")
            .WithSummary("Ajustar stock (Admin)")
            .RequireAuthorization("SoloAdmin");

        group.MapDelete("/{id:long}", async (long id, IComponenteService service, CancellationToken ct) =>
            {
                await service.DesactivarAsync(id, ct);
                return Results.NoContent();
            })
            .WithName("DesactivarComponente")
            .WithSummary("Desactivar componente (Admin)")
            .RequireAuthorization("SoloAdmin");

        return group;
    }
}