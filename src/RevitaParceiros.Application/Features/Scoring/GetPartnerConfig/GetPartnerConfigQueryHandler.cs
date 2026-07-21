using Mediator;
using RevitaParceiros.Domain.Enums;
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
                Ranges = new List<ScoringRangeDto>(),
                PointsForRedemption = 100,
                RedemptionValue = 50,
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            };
        }

        return new ScoringConfigDto
        {
            Ranges = config.FaixasPontuacao
                .Where(f => f.Tipo == TipoFaixaPontuacaoEnum.Parceiro)
                .OrderBy(f => f.ValorVendas)
                .Select(f => new ScoringRangeDto
                {
                    SalesThreshold = f.ValorVendas,
                    Points = f.Pontos
                })
                .ToList(),
            PointsForRedemption = config.PontosParaConversaoMonetariaParceiro,
            RedemptionValue = config.ValorMonetarioPorPontosParceiro,
            UpdatedAt = config.AtualizadoEm ?? config.CriadoEm
        };
    }
}
