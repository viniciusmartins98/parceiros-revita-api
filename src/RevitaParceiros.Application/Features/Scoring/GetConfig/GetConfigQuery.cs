using Mediator;

namespace RevitaParceiros.Application.Features.Scoring.GetConfig;

public sealed record GetConfigQuery() : IRequest<ScoringConfigDto>;
