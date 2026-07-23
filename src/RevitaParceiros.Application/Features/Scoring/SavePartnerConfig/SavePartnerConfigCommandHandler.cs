using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.SavePartnerConfig;

public sealed class SavePartnerConfigCommandHandler : IRequestHandler<SavePartnerConfigCommand, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;
    private readonly IFaixaPontuacaoRepository _faixaRepository;

    public SavePartnerConfigCommandHandler(IRegrasPontuacaoRepository repository, IFaixaPontuacaoRepository faixaRepository)
    {
        _repository = repository;
        _faixaRepository = faixaRepository;
    }

    public async ValueTask<ScoringConfigDto> Handle(SavePartnerConfigCommand request, CancellationToken cancellationToken)
    {
        var currentActive = await _repository.GetActiveConfigAsync(cancellationToken);

        if (currentActive != null)
        {
            currentActive.Ativo = false;
            currentActive.AtualizadoEm = DateTime.UtcNow;
            await _repository.UpdateAsync(currentActive, cancellationToken);
        }

        var newConfig = new RegrasPontuacao
        {
            Id = Guid.NewGuid(),
            Nome = "Configuração Padrão",
            PontosParaConversaoMonetariaParceiro = request.PointsForRedemption,
            ValorMonetarioPorPontosParceiro = request.RedemptionValue,
            ValorCompraMinimoCliente = currentActive?.ValorCompraMinimoCliente ?? 1000m,
            PontosPorValorCliente = currentActive?.PontosPorValorCliente ?? 100,
            PontosParaConversaoMonetariaCliente = currentActive?.PontosParaConversaoMonetariaCliente ?? 100,
            ValorMonetarioPorPontosCliente = currentActive?.ValorMonetarioPorPontosCliente ?? 50m,
            Ativo = true,
            CriadoPor = request.UserId,
            CriadoEm = DateTime.UtcNow
        };

        var faixas = new List<FaixasPontuacao>();

        // Partner ranges from request
        foreach (var range in request.Ranges)
        {
            faixas.Add(new FaixasPontuacao
            {
                RegraPontuacaoId = newConfig.Id,
                Tipo = TipoFaixaPontuacaoEnum.Parceiro,
                ValorVendas = range.SalesThreshold,
                Pontos = range.Points,
                CriadoEm = DateTime.UtcNow
            });
        }

        // Preserve client ranges from previous config if any
        if (currentActive != null && currentActive.FaixasPontuacao != null)
        {
            foreach (var clientFaixa in currentActive.FaixasPontuacao.Where(f => f.Tipo == TipoFaixaPontuacaoEnum.Cliente))
            {
                faixas.Add(new FaixasPontuacao
                {
                    RegraPontuacaoId = newConfig.Id,
                    Tipo = TipoFaixaPontuacaoEnum.Cliente,
                    ValorVendas = clientFaixa.ValorVendas,
                    Pontos = clientFaixa.Pontos,
                    CriadoEm = DateTime.UtcNow
                });
            }
        }

        await _repository.AddAsync(newConfig, cancellationToken);
        if (faixas.Any())
        {
            await _faixaRepository.AddRangeAsync(faixas, cancellationToken);
        }

        return new ScoringConfigDto
        {
            Ranges = request.Ranges.OrderBy(r => r.SalesThreshold).ToList(),
            PointsForRedemption = newConfig.PontosParaConversaoMonetariaParceiro,
            RedemptionValue = newConfig.ValorMonetarioPorPontosParceiro,
            UpdatedAt = newConfig.CriadoEm
        };
    }
}
