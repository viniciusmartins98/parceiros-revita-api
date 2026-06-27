using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.GetClientConfig;

public sealed record GetClientConfigQuery() : IRequest<ScoringConfigDto>;
