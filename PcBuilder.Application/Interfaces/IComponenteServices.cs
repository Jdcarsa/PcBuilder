using PcBuilder.Application.DTOs;
using PcBuilder.Domain.Enums;

namespace PcBuilder.Application.Interfaces;

public interface IComponenteService
{
    Task<List<ComponenteResponse>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<List<ComponenteResponse>> ObtenerPorCategoriaAsync(CategoriaComponente categoria, CancellationToken ct = default);
    Task<List<ComponenteResponse>> BuscarAsync(string termino, CancellationToken ct = default);
    Task<ComponenteResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<ComponenteResponse> CrearAsync(CrearComponenteRequest request, CancellationToken ct = default);
    Task ActualizarPrecioAsync(long id, ActualizarPrecioRequest request, CancellationToken ct = default);
    Task AjustarStockAsync(long id, AjustarStockRequest request, CancellationToken ct = default);
    Task DesactivarAsync(long id, CancellationToken ct = default);
}






