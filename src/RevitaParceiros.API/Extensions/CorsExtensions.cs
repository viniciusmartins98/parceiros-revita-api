namespace RevitaParceiros.API.Extensions;

/// <summary>
/// Extensões para configuração do CORS.
/// </summary>
public static class CorsExtensions
{
    public const string PolicyName = "RevitaCorsPolicy";

    /// <summary>
    /// Configura a política de CORS para permitir requisições do front-end Angular.
    /// </summary>
    public static IServiceCollection AddRevitaCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var allowedOrigins = configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? ["http://localhost:4200"];

        services.AddCors(options =>
        {
            options.AddPolicy(PolicyName, policy =>
            {
                policy
                    .WithOrigins(allowedOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }
}
