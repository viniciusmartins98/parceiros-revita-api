using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.SaveClientConfig;

public sealed record SaveClientConfigCommand(
    decimal PurchaseAmountPerPoint,
    int PointsGenerated,
    int PointsForRedemption,
    decimal RedemptionValue,
    Guid UserId
) : IRequest<ScoringConfigDto>;
