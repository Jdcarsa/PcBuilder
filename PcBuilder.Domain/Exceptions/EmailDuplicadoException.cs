namespace PcBuilder.Domain.Exceptions;

public class EmailDuplicadoException(string email)
    : Exception($"Ya existe un usuario con el email '{email}'.");