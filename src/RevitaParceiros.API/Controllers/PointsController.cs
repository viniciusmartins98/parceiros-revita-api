using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RevitaParceiros.Application.Features.Points.Dtos;
using RevitaParceiros.Application.Features.Points.RedeemPoints;
using RevitaParceiros.Application.Features.Points.SearchProfiles;
using RevitaParceiros.Application.Features.Points.GetRecentRedemptions;

namespace RevitaParceiros.API.Controllers;

[Authorize(Roles = "Administrador,Funcionario")]
public class PointsController(IServiceProvider provider) : ControllerBase<PointsController>(provider)
{
    [HttpGet("profiles")]
    public async Task<IActionResult> SearchProfiles([FromQuery] string? term, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new SearchProfilesQuery(term), cancellationToken);
        return Ok(result);
    }

    [HttpPost("redeem")]
    public async Task<IActionResult> RedeemPoints([FromBody] RedeemPointsRequestDto request, CancellationToken cancellationToken)
    {
        IRequest<RedeemPointsResponseDto> command = request.ProfileType == Domain.Enums.PerfilUsuarioEnum.Cliente
            ? new RedeemClientPointsCommand(request.ProfileId, request.Points, UserId!.Value)
            : new RedeemPartnerPointsCommand(request.ProfileId, request.Points, UserId!.Value);

        var result = await Mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("redemptions")]
    public async Task<IActionResult> GetRecentRedemptions([FromQuery] int limit = 50, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetRecentRedemptionsQuery(limit), cancellationToken);
        return Ok(result);
    }
}
