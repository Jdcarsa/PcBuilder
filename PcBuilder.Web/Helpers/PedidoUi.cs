namespace PcBuilder.Web.Helpers;

public static class PedidoUi
{
    public static string Badge(string estado) => estado switch
    {
        "Confirmado"      => "bg-primary",
        "EnProcesamiento" => "bg-warning text-dark",
        "Enviado"         => "bg-info text-dark",
        "Entregado"       => "bg-success",
        "Cancelado"       => "bg-danger",
        _                 => "bg-secondary"
    };
    
    public static string BadgeSutil(string estado) => estado switch
    {
        "Confirmado"      => "bg-primary-subtle text-primary",
        "EnProcesamiento" => "bg-warning-subtle text-warning-emphasis",
        "Enviado"         => "bg-info-subtle text-info-emphasis",
        "Entregado"       => "bg-success-subtle text-success-emphasis",
        "Cancelado"       => "bg-danger-subtle text-danger",
        _                 => "bg-secondary-subtle text-secondary"
    };

    public static string Icono(string estado) => estado switch
    {
        "Confirmado"      => "bi-check-circle",
        "EnProcesamiento" => "bi-gear",
        "Enviado"         => "bi-truck",
        "Entregado"       => "bi-bag-check",
        "Cancelado"       => "bi-x-circle",
        _                 => "bi-circle"
    };

    public static string NombrePaso(string paso) => paso switch
    {
        "Confirmado"      => "Confirmado",
        "EnProcesamiento" => "Procesando",
        "Enviado"         => "Enviado",
        "Entregado"       => "Entregado",
        _                 => paso
    };
}