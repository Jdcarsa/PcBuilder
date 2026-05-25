using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Enums;

namespace PcBuilder.Domain.Interfaces;

public interface IComponenteRepository
{
    Task<Componente?> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<List<Componente>> ObtenerTodosAsync(CancellationToken ct = default);
    Task<List<Componente>> ObtenerPorCategoriaAsync(CategoriaComponente categoria, CancellationToken ct = default);
    Task<List<Componente>> BuscarAsync(string termino, CancellationToken ct = default);
    Task AgregarAsync(Componente componente, CancellationToken ct = default);
    void Actualizar(Componente componente);
    void Eliminar(Componente componente);
}