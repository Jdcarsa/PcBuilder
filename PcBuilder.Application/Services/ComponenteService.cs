using PcBuilder.Application.DTOs;
using PcBuilder.Application.Interfaces;
using PcBuilder.Domain.Entities;
using PcBuilder.Domain.Enums;
using PcBuilder.Domain.Exceptions;
using PcBuilder.Domain.Interfaces;

namespace PcBuilder.Application.Services;

public class ComponenteService(
    IComponenteRepository componenteRepo,
    IUnitOfWork uow) : IComponenteService
{
    public async Task<List<ComponenteResponse>> ObtenerTodosAsync(CancellationToken ct = default)
    {
        var componentes = await componenteRepo.ObtenerTodosAsync(ct);
        return componentes.Select(c => c.ToResponse()).ToList();
    }

    public async Task<List<ComponenteResponse>> ObtenerPorCategoriaAsync(CategoriaComponente categoria, CancellationToken ct = default)
    {
        var componentes = await componenteRepo.ObtenerPorCategoriaAsync(categoria, ct);
        return componentes.Select(c => c.ToResponse()).ToList();
    }

    public async Task<List<ComponenteResponse>> BuscarAsync(string termino, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(termino))
            return await ObtenerTodosAsync(ct);

        var componentes = await componenteRepo.BuscarAsync(termino, ct);
        return componentes.Select(c => c.ToResponse()).ToList();
    }

    public async Task<ComponenteResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default)
    {
        var componente = await componenteRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Componente", id);

        return componente.ToResponse();
    }

    public async Task<ComponenteResponse> CrearAsync(CrearComponenteRequest request, CancellationToken ct = default)
    {
        var componente = Componente.Crear(
            request.Nombre,
            request.Descripcion,
            request.Sku,
            request.Marca,
            request.Modelo,
            request.Categoria,
            request.Precio,
            request.Stock,
            request.ConsumoWatts,
            request.UrlImagen);

        await componenteRepo.AgregarAsync(componente, ct);
        await uow.GuardarCambiosAsync(ct);

        return componente.ToResponse();
    }

    public async Task ActualizarPrecioAsync(long id, ActualizarPrecioRequest request, CancellationToken ct = default)
    {
        var componente = await componenteRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Componente", id);

        componente.ActualizarPrecio(request.NuevoPrecio);
        componenteRepo.Actualizar(componente);
        await uow.GuardarCambiosAsync(ct);
    }

    public async Task AjustarStockAsync(long id, AjustarStockRequest request, CancellationToken ct = default)
    {
        var componente = await componenteRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Componente", id);

        if (request.Cantidad > 0)
            componente.ReponerStock(request.Cantidad);
        else
            componente.DescontarStock(Math.Abs(request.Cantidad));

        componenteRepo.Actualizar(componente);
        await uow.GuardarCambiosAsync(ct);
    }

    public async Task DesactivarAsync(long id, CancellationToken ct = default)
    {
        var componente = await componenteRepo.ObtenerPorIdAsync(id, ct)
            ?? throw new EntidadNoEncontradaException("Componente", id);

        componente.Desactivar();
        componenteRepo.Actualizar(componente);
        await uow.GuardarCambiosAsync(ct);
    }
}
