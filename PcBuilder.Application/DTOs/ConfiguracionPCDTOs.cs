namespace PcBuilder.Application.DTOs;

public record ConfiguracionPCResponse(long Id, string Nombre, bool EstaFinalizada, decimal PrecioTotal,
    int ConsumoTotalWatts, DateTime CreadoEn, IReadOnlyList<ItemConfiguracionResponse> Items);

public record ItemConfiguracionResponse(long ComponenteId, string NombreComponente, string Categoria, decimal PrecioUnitario,
    int Cantidad, decimal Subtotal);

public record CrearConfiguracionRequest(string Nombre);

public record AgregarComponenteRequest(long ComponenteId, int Cantidad = 1);

public record ValidacionResponse(bool EsValida, IReadOnlyList<string> Errores);
