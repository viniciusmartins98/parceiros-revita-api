using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace RevitaParceiros.Application;

/// <summary>
/// Extensões de injeção de dependência para a camada Application.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da camada Application: Mediator, FluentValidation validators e pipeline behaviors.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Mediator é registrado via source generator — AddMediator() é auto-gerado
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        // Auto-discovery de todos os validators do assembly
        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true);

        return services;
    }
}
