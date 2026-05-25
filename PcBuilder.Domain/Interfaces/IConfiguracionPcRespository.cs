using PcBuilder.Domain.Entities;

namespace PcBuilder.Domain.Interfaces;

public interface IConfiguracionPcRespository
{
    Task<ConfiguracionPC?> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<List<ConfiguracionPC>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default);
    Task AgregarAsync(ConfiguracionPC configuracion, CancellationToken ct = default);
    void Actualizar(ConfiguracionPC configuracion);
    void Eliminar(ConfiguracionPC configuracion);
}