using System.Security.Claims;
using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;

namespace PcBuilder.Web.Endpoints;

public static class PedidoEndpoints
{
    public static RouteGroupBuilder MapPedidoEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/", async (ClaimsPrincipal user, IPedidoService service, CancellationToken ct) =>
        {
            var usuarioId = user.ObtenerUsuarioId();
            var pedidos = await service.ObtenerPorUsuarioAsync(usuarioId, ct);
            return Results.Ok(pedidos);
        })
        .WithName("GetPedidos")
        .WithSummary("Listar pedidos del usuario")
        .RequireAuthorization();
        
        group.MapGet("/todos", async (IPedidoService service, CancellationToken ct) =>
            {
                var pedidos = await service.ObtenerTodosAsync(ct);
                return Results.Ok(pedidos);
            })
            .WithName("GetTodosPedidos")
            .WithSummary("Listar todos los pedidos (Admin)")
            .RequireAuthorization("SoloAdmin");

        group.MapGet("/{id:long}", async (long id, IPedidoService service, CancellationToken ct) =>
        {
            var pedido = await service.ObtenerPorIdAsync(id, ct);
            return Results.Ok(pedido);
        })
        .WithName("GetPedido")
        .WithSummary("Obtener pedido por Id")
        .RequireAuthorization();

        group.MapPost("/", async (CrearPedidoRequest request, ClaimsPrincipal user, IPedidoService service,
            CancellationToken ct) =>
        {
            var usuarioId = user.ObtenerUsuarioId();
            var pedido = await service.CrearAsync(usuarioId, request, ct);
            return Results.Created($"/api/pedidos/{pedido.Id}", pedido);
        })
        .WithName("CrearPedido")
        .WithSummary("Crear nuevo pedido")
        .RequireAuthorization();

        group.MapPost("/{id:long}/items", async (long id, AgregarItemPedidoRequest request, IPedidoService service,
            CancellationToken ct) =>
        {
            var pedido = await service.AgregarItemAsync(id, request, ct);
            return Results.Ok(pedido);
        })
        .WithName("AgregarItemPedido")
        .WithSummary("Agregar item al pedido")
        .RequireAuthorization();

        group.MapPost("/{id:long}/confirmar", async (long id, IPedidoService service, CancellationToken ct) =>
        {
            var pedido = await service.ConfirmarAsync(id, ct);
            return Results.Ok(pedido);
        })
        .WithName("ConfirmarPedido")
        .WithSummary("Confirmar pedido")
        .RequireAuthorization();

        group.MapPost("/{id:long}/procesar", async (long id, IPedidoService service, CancellationToken ct) =>
        {
            var pedido = await service.IniciarProcesamientoAsync(id, ct);
            return Results.Ok(pedido);
        })
        .WithName("ProcesarPedido")
        .WithSummary("Iniciar procesamiento (Admin)")
        .RequireAuthorization("SoloAdmin");

        group.MapPost("/{id:long}/enviar", async (long id, EnviarPedidoRequest request, IPedidoService service,
            CancellationToken ct) =>
        {
            var pedido = await service.EnviarAsync(id, request, ct);
            return Results.Ok(pedido);
        })
        .WithName("EnviarPedido")
        .WithSummary("Marcar pedido como enviado (Admin)")
        .RequireAuthorization("SoloAdmin");

        group.MapPost("/{id:long}/entregar", async (long id, IPedidoService service, CancellationToken ct) =>
        {
            var pedido = await service.MarcarEntregadoAsync(id, ct);
            return Results.Ok(pedido);
        })
        .WithName("EntregarPedido")
        .WithSummary("Marcar pedido como entregado (Admin)")
        .RequireAuthorization("SoloAdmin");

        group.MapPost("/{id:long}/cancelar", async (long id, IPedidoService service, CancellationToken ct) =>
        {
            var pedido = await service.CancelarAsync(id, ct);
            return Results.Ok(pedido);
        })
        .WithName("CancelarPedido")
        .WithSummary("Cancelar pedido")
        .RequireAuthorization();

        return group;
    }
}
