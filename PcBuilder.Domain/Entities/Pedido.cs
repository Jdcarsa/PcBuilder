// Pedido.cs
using PcBuilder.Domain.Enums;

namespace PcBuilder.Domain.Entities;

public class Pedido
{
    public long Id { get; private set; }
    public string NumeroPedido { get; private set; } = string.Empty;
    public long UsuarioId { get; private set; }
    public EstadoPedido Estado { get; private set; }
    public string DireccionEnvio { get; private set; } = string.Empty;
    public string? NotasCliente { get; private set; }
    public string? NumeroGuia { get; private set; }
    public DateTime CreadoEn { get; private set; }

    private readonly List<ItemPedido> _items = [];
    public IReadOnlyList<ItemPedido> Items => _items.AsReadOnly();

    public decimal Total => _items.Sum(i => i.PrecioUnitario * i.Cantidad);

    private Pedido() { }

    public static Pedido Crear(long usuarioId, string direccionEnvio, string? notas = null)
    {
        if (string.IsNullOrWhiteSpace(direccionEnvio))
            throw new ArgumentException("La dirección es requerida.");

        return new Pedido
        {
            NumeroPedido = $"PCB-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}",
            UsuarioId = usuarioId,
            Estado = EstadoPedido.Borrador,
            DireccionEnvio = direccionEnvio.Trim(),
            NotasCliente = notas,
            CreadoEn = DateTime.UtcNow
        };
    }

    public void AgregarItem(Componente componente, int cantidad)
    {
        if (Estado != EstadoPedido.Borrador)
            throw new InvalidOperationException("Solo se pueden agregar items a pedidos en borrador.");

        if (!componente.TieneStock(cantidad))
            throw new InvalidOperationException($"No hay stock suficiente para '{componente.Nombre}'.");

        var existente = _items.FirstOrDefault(i => i.ComponenteId == componente.Id);
        if (existente is not null)
            existente.AumentarCantidad(cantidad);
        else
            _items.Add(new ItemPedido(componente, cantidad));
    }

    public void AgregarDesdeConfiguracion(ConfiguracionPC configuracion)
    {
        if (!configuracion.EstaFinalizada)
            throw new InvalidOperationException("La configuración debe estar finalizada.");

        foreach (var item in configuracion.Items)
            AgregarItem(item.Componente, item.Cantidad);
    }

    public void Confirmar()
    {
        if (Estado != EstadoPedido.Borrador)
            throw new InvalidOperationException("Solo se puede confirmar un pedido en borrador.");

        if (!_items.Any())
            throw new InvalidOperationException("El pedido no tiene items.");

        Estado = EstadoPedido.Confirmado;
    }

    public void IniciarProcesamiento()
    {
        if (Estado != EstadoPedido.Confirmado)
            throw new InvalidOperationException("El pedido debe estar confirmado.");

        Estado = EstadoPedido.EnProcesamiento;
    }

    public void Enviar(string numeroGuia)
    {
        if (Estado != EstadoPedido.EnProcesamiento)
            throw new InvalidOperationException("El pedido debe estar en procesamiento.");

        if (string.IsNullOrWhiteSpace(numeroGuia))
            throw new ArgumentException("El número de guía es requerido.");

        Estado = EstadoPedido.Enviado;
        NumeroGuia = numeroGuia.Trim();
    }

    public void MarcarEntregado()
    {
        if (Estado != EstadoPedido.Enviado)
            throw new InvalidOperationException("El pedido debe estar enviado.");

        Estado = EstadoPedido.Entregado;
    }

    public void Cancelar()
    {
        if (Estado is EstadoPedido.Enviado or EstadoPedido.Entregado)
            throw new InvalidOperationException("No se puede cancelar un pedido enviado o entregado.");

        Estado = EstadoPedido.Cancelado;
    }

    public bool PuedeModificarse() => Estado == EstadoPedido.Borrador;
}