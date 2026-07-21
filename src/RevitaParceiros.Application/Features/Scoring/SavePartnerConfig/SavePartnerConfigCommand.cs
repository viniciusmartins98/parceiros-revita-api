using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.SavePartnerConfig;

public sealed record SavePartnerConfigCommand(
    List<ScoringRangeDto> Ranges,
    int PointsForRedemption,
    decimal RedemptionValue,
    Guid UserId
) : IRequest<ScoringConfigDto>;
