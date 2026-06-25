using Microsoft.AspNetCore.Mvc;

namespace RevitaParceiros.API.Controllers;

/// <summary>
/// Controller de verificação de saúde da API.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public sealed class HealthController(IServiceProvider provider) : ControllerBase<HealthController>(provider)
{
    /// <summary>
    /// Verifica se a API está online e respondendo.
    /// </summary>
    /// <returns>Status da API.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new HealthResponse(
            Status: "Healthy",
            Timestamp: DateTime.UtcNow,
            Version: "1.0.0"));
    }
}

/// <summary>
/// Resposta do health check.
/// </summary>
public sealed record HealthResponse(
    string Status,
    DateTime Timestamp,
    string Version);
