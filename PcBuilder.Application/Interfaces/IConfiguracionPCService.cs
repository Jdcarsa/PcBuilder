using PcBuilder.Application.DTOs;

namespace PcBuilder.Application.Interfaces;

public interface IConfiguracionPCService
{
    Task<List<ConfiguracionPCResponse>> ObtenerPorUsuarioAsync(long usuarioId, CancellationToken ct = default);
    Task<ConfiguracionPCResponse> ObtenerPorIdAsync(long id, CancellationToken ct = default);
    Task<ConfiguracionPCResponse> CrearAsync(long usuarioId, CrearConfiguracionRequest request, CancellationToken ct = default);
    Task<ConfiguracionPCResponse> AgregarComponenteAsync(long configuracionId, AgregarComponenteRequest request, CancellationToken ct = default);
    Task<ConfiguracionPCResponse> RemoverComponenteAsync(long configuracionId, long componenteId, CancellationToken ct = default);
    Task<ValidacionResponse> ValidarCompatibilidadAsync(long configuracionId, CancellationToken ct = default);
    Task<ConfiguracionPCResponse> FinalizarAsync(long configuracionId, CancellationToken ct = default);
    Task EliminarAsync(long configuracionId, CancellationToken ct = default);
}