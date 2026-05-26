using Microsoft.Extensions.DependencyInjection;
using PcBuilder.Application.Interfaces;
using PcBuilder.Application.Services;

namespace PcBuilder.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IComponenteService, ComponenteService>();
        services.AddScoped<IUsuarioService, UsuarioService>();
        services.AddScoped<IConfiguracionPCService, ConfiguracionPCService>();
        services.AddScoped<IPedidoService, PedidoService>();

        return services;
    }
}
