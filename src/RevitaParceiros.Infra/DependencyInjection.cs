using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Infra.Persistence;
using RevitaParceiros.Infra.Services;

namespace RevitaParceiros.Infra;

/// <summary>
/// Extensões de injeção de dependência para a camada de Infraestrutura.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registra os serviços da camada Infra: DbContext, repositórios e services externos.
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Entity Framework Core — PostgreSQL
        services.AddDbContext<DatabaseContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(typeof(DatabaseContext).Assembly.FullName);
                    npgsqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: null);
                }));

        // Services
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        // Auth Repositories & Services
        services.AddScoped<Domain.Interfaces.IUsuarioRepository, RevitaParceiros.Infra.Persistence.Repositories.UsuarioRepository>();
        services.AddScoped<Domain.Interfaces.IParceiroRepository, RevitaParceiros.Infra.Persistence.Repositories.ParceiroRepository>();
        services.AddScoped<Domain.Interfaces.IFuncionarioRepository, RevitaParceiros.Infra.Persistence.Repositories.FuncionarioRepository>();
        services.AddScoped<Domain.Interfaces.IClienteRepository, RevitaParceiros.Infra.Persistence.Repositories.ClienteRepository>();
        services.AddScoped<Domain.Interfaces.IRefreshTokenRepository, RevitaParceiros.Infra.Persistence.Repositories.RefreshTokenRepository>();
        services.AddScoped<Domain.Interfaces.IRegrasPontuacaoRepository, RevitaParceiros.Infra.Persistence.Repositories.RegrasPontuacaoRepository>();
        services.AddScoped<Domain.Interfaces.ICompraRepository, RevitaParceiros.Infra.Persistence.Repositories.CompraRepository>();
        services.AddSingleton<IJwtTokenService, JwtTokenService>();
        services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();

        return services;
    }
}
