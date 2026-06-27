using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.GetConfig;

public sealed class GetConfigQueryHandler : IRequestHandler<GetConfigQuery, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;

    public GetConfigQueryHandler(IRegrasPontuacaoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<ScoringConfigDto> Handle(GetConfigQuery request, CancellationToken cancellationToken)
    {
        var config = await _repository.GetActiveConfigAsync(cancellationToken);

        if (config == null)
        {
            // Default fallback if nothing is configured
            return new ScoringConfigDto
            {
                PurchaseAmountPerPoint = 1000,
                PointsGenerated = 100,
                PointsForRedemption = 100,
                RedemptionValue = 50,
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
        }

        return new ScoringConfigDto
        {
            PurchaseAmountPerPoint = config.ValorCompraMinimo,
            PointsGenerated = config.PontosPorValor,
            PointsForRedemption = config.PontosParaConversaoMonetaria,
            RedemptionValue = config.ValorMonetarioPorPontos,
            UpdatedAt = config.AtualizadoEm ?? config.CriadoEm
        };
    }
}
