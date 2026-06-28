using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Scoring;
using RevitaParceiros.Application.Features.Scoring.GetClientConfig;
using RevitaParceiros.Application.Features.Scoring.GetPartnerConfig;
using RevitaParceiros.Application.Features.Scoring.SaveClientConfig;
using RevitaParceiros.Application.Features.Scoring.SavePartnerConfig;

namespace RevitaParceiros.API.Controllers;

public class ScoringController(IServiceProvider provider) : ControllerBase<ScoringController>(provider)
{
    [HttpGet("partner-config")]
    public async Task<IActionResult> GetPartnerConfig(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetPartnerConfigQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("partner-config")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SavePartnerConfig([FromBody] ScoringConfigDto request, CancellationToken cancellationToken)
    {
        var command = new SavePartnerConfigCommand(
            request.PurchaseAmountPerPoint,
            request.PointsGenerated,
            request.PointsForRedemption,
            request.RedemptionValue,
            UserId!.Value
        );

        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("client-config")]
    public async Task<IActionResult> GetClientConfig(CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetClientConfigQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpPut("client-config")]
    [Authorize(Roles = "Administrador")]
    public async Task<IActionResult> SaveClientConfig([FromBody] ScoringConfigDto request, CancellationToken cancellationToken)
    {
        var command = new SaveClientConfigCommand(
            request.PurchaseAmountPerPoint,
            request.PointsGenerated,
            request.PointsForRedemption,
            request.RedemptionValue,
            UserId!.Value
        );

        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}
