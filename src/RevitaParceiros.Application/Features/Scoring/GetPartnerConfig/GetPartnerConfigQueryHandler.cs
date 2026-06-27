using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.GetPartnerConfig;

public sealed class GetPartnerConfigQueryHandler : IRequestHandler<GetPartnerConfigQuery, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;

    public GetPartnerConfigQueryHandler(IRegrasPontuacaoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<ScoringConfigDto> Handle(GetPartnerConfigQuery request, CancellationToken cancellationToken)
    {
        var config = await _repository.GetActiveConfigAsync(cancellationToken);

        if (config == null)
        {
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
            PurchaseAmountPerPoint = config.ValorCompraMinimoParceiro,
            PointsGenerated = config.PontosPorValorParceiro,
            PointsForRedemption = config.PontosParaConversaoMonetariaParceiro,
            RedemptionValue = config.ValorMonetarioPorPontosParceiro,
            UpdatedAt = config.AtualizadoEm ?? config.CriadoEm
        };
    }
}
