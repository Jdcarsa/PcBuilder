namespace PcBuilder.Application.DTOs;

public record PedidoResponse(long Id, string NumeroPedido, string Estado, string DireccionEnvio, string? NotasCliente,
    string? NumeroGuia, decimal Total, DateTime CreadoEn, IReadOnlyList<ItemPedidoResponse> Items);

public record ItemPedidoResponse(long ComponenteId, string NombreComponente, decimal PrecioUnitario,
    int Cantidad, decimal Subtotal);

public record CrearPedidoRequest(string DireccionEnvio, string? NotasCliente, long? ConfiguracionId);

public record AgregarItemPedidoRequest(long ComponenteId, int Cantidad);

public record EnviarPedidoRequest(string NumeroGuia);
