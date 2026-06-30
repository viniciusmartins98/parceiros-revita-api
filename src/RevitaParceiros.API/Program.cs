using RevitaParceiros.API.Extensions;
using RevitaParceiros.API.Middlewares;
using RevitaParceiros.Application;
using RevitaParceiros.Infra;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// Service Registration
// ========================================

// Clean Architecture layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// API services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });
builder.Services.AddRevitaCors(builder.Configuration);
builder.Services.AddRevitaAuth(builder.Configuration);
builder.Services.AddRevitaSwagger();

var app = builder.Build();

// ========================================
// Middleware Pipeline
// ========================================

// Global exception handling (primeiro na pipeline)
app.UseMiddleware<GlobalExceptionMiddleware>();

// Swagger (apenas em Development)
app.UseRevitaSwagger();

// CORS
app.UseCors(CorsExtensions.PolicyName);

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();

// Culture
var BR = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentCulture = BR;
CultureInfo.DefaultThreadCurrentUICulture = BR;

app.Run();
