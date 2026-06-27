using System.Security.Claims;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Scoring.GetConfig;
using RevitaParceiros.Application.Features.Scoring.SaveConfig;

namespace RevitaParceiros.API.Controllers;

public class ScoringController(IServiceProvider provider) : ControllerBase<ScoringController>(provider)
{
    [HttpGet("config")]
    public async Task<IActionResult> GetConfig(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetConfigQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("config")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SaveConfig([FromBody] UpdateScoringRequest request, CancellationToken cancellationToken)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var command = new SaveConfigCommand(
            request.PurchaseAmountPerPoint,
            request.PointsGenerated,
            request.PointsForRedemption,
            request.RedemptionValue,
            userId
        );

        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}

public class UpdateScoringRequest
{
    public decimal PurchaseAmountPerPoint { get; set; }
    public int PointsGenerated { get; set; }
    public int PointsForRedemption { get; set; }
    public decimal RedemptionValue { get; set; }
}
