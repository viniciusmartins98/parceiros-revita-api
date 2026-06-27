using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.SavePartnerConfig;

public sealed record SavePartnerConfigCommand(
    decimal PurchaseAmountPerPoint,
    int PointsGenerated,
    int PointsForRedemption,
    decimal RedemptionValue,
    Guid UserId
) : IRequest<ScoringConfigDto>;
