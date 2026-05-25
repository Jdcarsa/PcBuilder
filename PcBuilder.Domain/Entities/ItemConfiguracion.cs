namespace PcBuilder.Domain.Entities;

public class ItemConfiguracion
{
    public long Id { get; private set; }
    public long ConfiguracionId { get; private set; }
    public long ComponenteId { get; private set; }
    public Componente Componente { get; private set; } = default!;
    public int Cantidad { get; private set; }

    private ItemConfiguracion() { }

    internal ItemConfiguracion(Componente componente, int cantidad)
    {
        ComponenteId = componente.Id;
        Componente = componente;
        Cantidad = cantidad;
    }

    internal void AumentarCantidad(int cantidad) => Cantidad += cantidad;
}

public record ResultadoValidacion(bool EsValida, IReadOnlyList<string> Errores);