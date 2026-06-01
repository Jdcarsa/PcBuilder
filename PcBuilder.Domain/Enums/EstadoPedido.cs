using System.ComponentModel.DataAnnotations;

namespace PcBuilder.Domain.Enums;

public enum EstadoPedido
{
    [Display(Name = "Borrador")]
    Borrador,
    [Display(Name = "Confirmado")]
    Confirmado,
    [Display(Name = "En Procesamiento")]
    EnProcesamiento,
    [Display(Name = "Enviado")]
    Enviado,
    [Display(Name = "Entregado")]
    Entregado,
    [Display(Name = "Cancelado")]
    Cancelado
}