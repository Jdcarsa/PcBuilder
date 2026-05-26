using PcBuilder.Domain.Enums;
using PcBuilder.Domain.Exceptions;

namespace PcBuilder.Domain.Entities;

public class Componente
{
    public long Id { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public string Descripcion { get; private set; } = string.Empty;
    public string Sku { get; private set; } = string.Empty;
    public string Marca { get; private set; } = string.Empty;
    public string Modelo { get; private set; } = string.Empty;
    public CategoriaComponente Categoria { get; private set; }
    public decimal Precio { get; private set; }
    public int Stock { get; private set; }
    public int ConsumoWatts { get; private set; }
    public string? UrlImagen { get; private set; }
    public bool EstaActivo { get; private set; }
    public DateTime CreadoEn { get; private set; }

    private Componente() { }

    public static Componente Crear(string nombre, string descripcion, string sku, string marca, string modelo,
        CategoriaComponente categoria, decimal precio, int stock, int consumoWatts, string? urlImagen = null)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");
        if (precio < 0) throw new ArgumentException("El precio no puede ser negativo.");
        if (stock < 0) throw new ArgumentException("El stock no puede ser negativo.");

        return new Componente
        {
            Id = 0,
            Nombre = nombre.Trim(),
            Descripcion = descripcion.Trim(),
            Sku = sku.ToUpperInvariant().Trim(),
            Marca = marca.Trim(),
            Modelo = modelo.Trim(),
            Categoria = categoria,
            Precio = precio,
            Stock = stock,
            ConsumoWatts = consumoWatts,
            UrlImagen = urlImagen,
            EstaActivo = true,
            CreadoEn = DateTime.UtcNow
        };
    }

    public void ActualizarPrecio(decimal nuevoPrecio)
    {
        if (nuevoPrecio < 0) throw new ArgumentException("El precio no puede ser negativo.");
        Precio = nuevoPrecio;
    }

    public void DescontarStock(int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
        if (Stock < cantidad) throw new StockInsuficienteException(Nombre, cantidad, Stock);
        Stock -= cantidad;
    }

    public void ReponerStock(int cantidad)
    {
        if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
        Stock += cantidad;
    }

    public bool TieneStock(int cantidad = 1) => EstaActivo && Stock >= cantidad;

    public void Desactivar() => EstaActivo = false;

    public void Activar() => EstaActivo = true;
}
