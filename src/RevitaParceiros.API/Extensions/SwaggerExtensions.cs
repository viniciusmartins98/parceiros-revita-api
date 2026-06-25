using Microsoft.OpenApi;

namespace RevitaParceiros.API.Extensions;

/// <summary>
/// Extensões para configuração do Swagger/OpenAPI com suporte a JWT.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Configura o Swagger com suporte a autenticação JWT Bearer.
    /// </summary>
    public static IServiceCollection AddRevitaSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Revita Parceiros API",
                Version = "v1",
                Description = "API do sistema de fidelidade e acompanhamento de compras Revita Parceiros."
            });

            // Configuração do JWT no Swagger
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT. Exemplo: eyJhbGciOiJIUzI1NiIs..."
            };

            options.AddSecurityDefinition("Bearer", securityScheme);

            options.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecuritySchemeReference("Bearer"),
                    new List<string>()
                }
            });

            // Incluir XML comments para documentação automática
            var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (var xmlFile in xmlFiles)
            {
                options.IncludeXmlComments(xmlFile, includeControllerXmlComments: true);
            }
        });

        return services;
    }

    /// <summary>
    /// Configura o middleware do Swagger UI.
    /// </summary>
    public static WebApplication UseRevitaSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Revita Parceiros API v1");
                options.RoutePrefix = "swagger";
            });
        }

        return app;
    }
}
