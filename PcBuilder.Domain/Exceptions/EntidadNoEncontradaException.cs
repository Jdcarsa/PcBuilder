namespace PcBuilder.Domain.Exceptions;

public class EntidadNoEncontradaException(string entidad, long id)
    : Exception($"{entidad} con Id '{id}' no encontrado.");