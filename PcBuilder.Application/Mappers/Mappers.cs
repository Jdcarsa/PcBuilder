using PcBuilder.Domain.Entities;

namespace PcBuilder.Application.DTOs;

public static class Mappers
{
    public static ComponenteResponse ToResponse(this Componente c) => new(c.Id, c.Nombre, c.Descripcion, c.Sku, c.Marca,
        c.Modelo, c.Categoria.ToString(), c.Precio, c.Stock, c.ConsumoWatts, c.UrlImagen, c.EstaActivo, c.TieneStock());

    public static UsuarioResponse ToResponse(this Usuario u) => new(u.Id, u.Nombre, u.Apellido, u.Email,
        u.NombreCompleto, u.Rol.ToString(), u.EstaActivo);

    public static ConfiguracionPCResponse ToResponse(this ConfiguracionPC c) => new(c.Id, c.Nombre, c.EstaFinalizada,
        c.PrecioTotal, c.ConsumoTotalWatts, c.CreadoEn,
        c.Items.Select(i => new ItemConfiguracionResponse(
            i.ComponenteId,
            i.Componente.Nombre,
            i.Componente.Categoria.ToString(),
            i.Componente.Precio,
            i.Cantidad,
            i.Componente.Precio * i.Cantidad)).ToList());

    public static PedidoResponse ToResponse(this Pedido p) => new(p.Id, p.NumeroPedido, p.Estado.ToString(),
        p.DireccionEnvio, p.NotasCliente, p.NumeroGuia, p.Total, p.CreadoEn,
        p.Items.Select(i => new ItemPedidoResponse(
            i.ComponenteId,
            i.NombreComponente,
            i.PrecioUnitario,
            i.Cantidad,
            i.Subtotal)).ToList());
}
