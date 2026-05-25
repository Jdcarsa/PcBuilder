using PcBuilder.Domain.Enums;

namespace PcBuilder.Domain.Entities;

public class ConfiguracionPC
{
    public long Id { get; private set; }
    public long UsuarioId { get; private set; }
    public string Nombre { get; private set; } = string.Empty;
    public bool EstaFinalizada { get; private set; }
    public DateTime CreadoEn { get; private set; }

    private readonly List<ItemConfiguracion> _items = [];
    public IReadOnlyList<ItemConfiguracion> Items => _items.AsReadOnly();

    public decimal PrecioTotal => _items.Sum(i => i.Componente.Precio * i.Cantidad);
    public int ConsumoTotalWatts => _items.Sum(i => i.Componente.ConsumoWatts * i.Cantidad);

    private ConfiguracionPC() { }

    public static ConfiguracionPC Crear(long usuarioId, string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("El nombre es requerido.");

        return new ConfiguracionPC
        {
            Id = 0,
            UsuarioId = usuarioId,
            Nombre = nombre.Trim(),
            EstaFinalizada = false,
            CreadoEn = DateTime.UtcNow
        };
    }

    public void AgregarComponente(Componente componente, int cantidad = 1)
    {
        if (EstaFinalizada)
            throw new InvalidOperationException("No se puede modificar una configuración finalizada.");

        if (cantidad <= 0)
            throw new ArgumentException("La cantidad debe ser mayor a cero.");

        var existente = _items.FirstOrDefault(i => i.ComponenteId == componente.Id);
        if (existente is not null)
            existente.AumentarCantidad(cantidad);
        else
            _items.Add(new ItemConfiguracion(componente, cantidad));
    }

    public void RemoverComponente(long componenteId)
    {
        if (EstaFinalizada)
            throw new InvalidOperationException("No se puede modificar una configuración finalizada.");

        var item = _items.FirstOrDefault(i => i.ComponenteId == componenteId);
        if (item is null) throw new InvalidOperationException("El componente no está en la configuración.");

        _items.Remove(item);
    }

    public ResultadoValidacion ValidarCompatibilidad()
    {
        var errores = new List<string>();

        var categorias = _items.Select(i => i.Componente.Categoria).ToList();

        if (!categorias.Contains(CategoriaComponente.Procesador))
            errores.Add("Falta el procesador.");

        if (!categorias.Contains(CategoriaComponente.PlacaMadre))
            errores.Add("Falta la placa madre.");

        if (!categorias.Contains(CategoriaComponente.MemoriaRam))
            errores.Add("Falta la memoria RAM.");

        if (!categorias.Contains(CategoriaComponente.Almacenamiento))
            errores.Add("Falta el almacenamiento.");

        if (!categorias.Contains(CategoriaComponente.FuentePoder))
            errores.Add("Falta la fuente de poder.");

        if (categorias.Count(c => c == CategoriaComponente.Procesador) > 1)
            errores.Add("Solo se permite un procesador.");

        if (categorias.Count(c => c == CategoriaComponente.PlacaMadre) > 1)
            errores.Add("Solo se permite una placa madre.");

        var fuentePoder = _items
            .FirstOrDefault(i => i.Componente.Categoria == CategoriaComponente.FuentePoder);

        if (fuentePoder is not null && ConsumoTotalWatts > fuentePoder.Componente.ConsumoWatts)
            errores.Add($"La fuente de poder ({fuentePoder.Componente.ConsumoWatts}W) es insuficiente para el consumo total ({ConsumoTotalWatts}W).");

        return new ResultadoValidacion(errores.Count == 0, errores);
    }

    public void Finalizar()
    {
        var validacion = ValidarCompatibilidad();
        if (!validacion.EsValida)
            throw new InvalidOperationException(
                $"La configuración no es válida: {string.Join(" | ", validacion.Errores)}");

        EstaFinalizada = true;
    }

    public void Renombrar(string nuevoNombre)
    {
        if (string.IsNullOrWhiteSpace(nuevoNombre)) throw new ArgumentException("El nombre es requerido.");
        Nombre = nuevoNombre.Trim();
    }
}

public class ItemConfiguracion
{
    public long ComponenteId { get; private set; }
    public Componente Componente { get; private set; }
    public int Cantidad { get; private set; }

    internal ItemConfiguracion(Componente componente, int cantidad)
    {
        Componente = componente;
        ComponenteId = componente.Id;
        Cantidad = cantidad;
    }

    internal void AumentarCantidad(int cantidad) => Cantidad += cantidad;
}

public record ResultadoValidacion(bool EsValida, IReadOnlyList<string> Errores);
