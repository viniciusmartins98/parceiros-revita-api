using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.GetClientConfig;

public sealed class GetClientConfigQueryHandler : IRequestHandler<GetClientConfigQuery, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;

    public GetClientConfigQueryHandler(IRegrasPontuacaoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<ScoringConfigDto> Handle(GetClientConfigQuery request, CancellationToken cancellationToken)
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
            PurchaseAmountPerPoint = config.ValorCompraMinimoCliente,
            PointsGenerated = config.PontosPorValorCliente,
            PointsForRedemption = config.PontosParaConversaoMonetariaCliente,
            RedemptionValue = config.ValorMonetarioPorPontosCliente,
            UpdatedAt = config.AtualizadoEm ?? config.CriadoEm
        };
    }
}
