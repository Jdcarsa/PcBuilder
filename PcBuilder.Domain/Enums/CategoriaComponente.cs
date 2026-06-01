using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PcBuilder.Domain.Enums;

public enum CategoriaComponente
{
    [Display(Name = "Procesador")]
    Procesador,
    [Display(Name = "Placa Madre")]
    PlacaMadre,
    [Display(Name = "Memoria RAM")]
    MemoriaRam,
    [Display(Name = "Almacenamiento")]
    Almacenamiento,
    [Display(Name = "Tarjeta Grafica")]
    TarjetaGrafica,
    [Display(Name = "Fuente de Poder")]
    FuentePoder,
    [Display(Name = "Gabinete")]
    Gabinete,
    [Display(Name = "Refrigeración")]
    Refrigeracion
}