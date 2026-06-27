using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.SaveConfig;

public sealed record SaveConfigCommand(
    decimal PurchaseAmountPerPoint,
    int PointsGenerated,
    int PointsForRedemption,
    decimal RedemptionValue,
    Guid UserId
) : IRequest<ScoringConfigDto>;
