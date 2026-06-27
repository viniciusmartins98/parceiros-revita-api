using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.GetPartnerConfig;

public sealed record GetPartnerConfigQuery() : IRequest<ScoringConfigDto>;
