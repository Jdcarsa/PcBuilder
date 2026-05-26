using PcBuilder.Domain.Enums;

namespace PcBuilder.Application.DTOs;

public record ComponenteResponse(
    long Id,
    string Nombre,
    string Descripcion,
    string Sku,
    string Marca,
    string Modelo,
    string Categoria,
    decimal Precio,
    int Stock,
    int ConsumoWatts,
    string? UrlImagen,
    bool EstaActivo,
    bool TieneStock);

public record CrearComponenteRequest(
    string Nombre,
    string Descripcion,
    string Sku,
    string Marca,
    string Modelo,
    CategoriaComponente Categoria,
    decimal Precio,
    int Stock,
    int ConsumoWatts,
    string? UrlImagen);

public record ActualizarPrecioRequest(decimal NuevoPrecio);

public record AjustarStockRequest(int Cantidad);
