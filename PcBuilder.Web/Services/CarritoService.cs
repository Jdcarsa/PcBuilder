using PcBuilder.Application.DTOs;

namespace PcBuilder.Web.Services;

public class CarritoItem
{
    public long ComponenteId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string? UrlImagen { get; set; }
    public decimal Precio { get; set; }
    public int Cantidad { get; set; }
    public decimal Subtotal => Precio * Cantidad;
}

public class CarritoService
{
    private readonly List<CarritoItem> _items = [];

    public IReadOnlyList<CarritoItem> Items => _items.AsReadOnly();
    public int TotalItems => _items.Sum(i => i.Cantidad);
    public decimal Total => _items.Sum(i => i.Subtotal);

    public event Action? OnChange;

    public void Agregar(ComponenteResponse componente, int cantidad = 1)
    {
        var existente = _items.FirstOrDefault(i => i.ComponenteId == componente.Id);
        if (existente is not null)
            existente.Cantidad += cantidad;
        else
            _items.Add(new CarritoItem
            {
                ComponenteId = componente.Id,
                Nombre = componente.Nombre,
                Categoria = componente.Categoria,
                UrlImagen = componente.UrlImagen,
                Precio = componente.Precio,
                Cantidad = cantidad
            });

        Notificar();
    }

    public void Remover(long componenteId)
    {
        var item = _items.FirstOrDefault(i => i.ComponenteId == componenteId);
        if (item is not null)
        {
            _items.Remove(item);
            Notificar();
        }
    }

    public void CambiarCantidad(long componenteId, int cantidad)
    {
        var item = _items.FirstOrDefault(i => i.ComponenteId == componenteId);
        if (item is null) return;

        if (cantidad <= 0)
            _items.Remove(item);
        else
            item.Cantidad = cantidad;

        Notificar();
    }

    public void Limpiar()
    {
        _items.Clear();
        Notificar();
    }

    public bool Contiene(long componenteId) =>
        _items.Any(i => i.ComponenteId == componenteId);

    private void Notificar() => OnChange?.Invoke();
}
