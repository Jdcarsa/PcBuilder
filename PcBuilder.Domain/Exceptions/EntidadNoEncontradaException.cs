namespace PcBuilder.Domain.Exceptions;

public class EntidadNoEncontradaException(string entidad, Guid id)
    : Exception($"{entidad} con Id '{id}' no encontrado.");