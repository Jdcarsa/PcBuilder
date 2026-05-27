namespace PcBuilder.Domain.Entities;

public class ItemPedido
{
    public long Id { get; private set; }
    public long PedidoId { get; private set; }
    public long ComponenteId { get; private set; }
    public Componente Componente { get; private set; } = default!;
    public string NombreComponente { get; private set; } = string.Empty;
    public decimal PrecioUnitario { get; private set; }
    public int Cantidad { get; private set; }

    public decimal Subtotal => PrecioUnitario * Cantidad;
    
    private ItemPedido() { }

    internal ItemPedido(Componente componente, int cantidad)
    {
        ComponenteId = componente.Id;
        Componente = componente;
        NombreComponente = componente.Nombre;
        PrecioUnitario = componente.Precio;
        Cantidad = cantidad;
    }

    internal void AumentarCantidad(int cantidad) => Cantidad += cantidad;
}