namespace PcBuilder.Domain.Exceptions;

public class StockInsuficienteException(string nombre, int requerido, int disponible)
    : Exception($"Stock insuficiente para '{nombre}'. Requerido: {requerido}, Disponible: {disponible}.");