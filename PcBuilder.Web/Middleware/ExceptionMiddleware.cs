using System.Net;
using System.Text.Json;
using PcBuilder.Domain.Exceptions;

namespace PcBuilder.Web.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error no controlado: {Mensaje}", ex.Message);
            await ManejarExcepcionAsync(context, ex);
        }
    }

    private static async Task ManejarExcepcionAsync(HttpContext context, Exception ex)
    {
        var (statusCode, mensaje) = ex switch
        {
            EntidadNoEncontradaException => (HttpStatusCode.NotFound, ex.Message),
            EmailDuplicadoException => (HttpStatusCode.Conflict, ex.Message),
            StockInsuficienteException => (HttpStatusCode.BadRequest, ex.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, ex.Message),
            ArgumentException => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "Ocurrió un error interno. Intente más tarde.")
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var respuesta = new ErrorResponse((int)statusCode, mensaje);
        var json = JsonSerializer.Serialize(respuesta, JsonOpciones);
        await context.Response.WriteAsync(json);
    }

    private static readonly JsonSerializerOptions JsonOpciones = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };
}

public record ErrorResponse(int Status, string Mensaje);
