using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.SaveClientConfig;

public sealed class SaveClientConfigCommandHandler : IRequestHandler<SaveClientConfigCommand, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;
    private readonly IFaixaPontuacaoRepository _faixaRepository;

    public SaveClientConfigCommandHandler(IRegrasPontuacaoRepository repository, IFaixaPontuacaoRepository faixaRepository)
    {
        _repository = repository;
        _faixaRepository = faixaRepository;
    }

    public async ValueTask<ScoringConfigDto> Handle(SaveClientConfigCommand request, CancellationToken cancellationToken)
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
            PontosParaConversaoMonetariaParceiro = currentActive?.PontosParaConversaoMonetariaParceiro ?? 100,
            ValorMonetarioPorPontosParceiro = currentActive?.ValorMonetarioPorPontosParceiro ?? 50m,
            ValorCompraMinimoCliente = request.PurchaseAmountPerPoint,
            PontosPorValorCliente = request.PointsGenerated,
            PontosParaConversaoMonetariaCliente = request.PointsForRedemption,
            ValorMonetarioPorPontosCliente = request.RedemptionValue,
            Ativo = true,
            CriadoPor = request.UserId,
            CriadoEm = DateTime.UtcNow
        };

        var faixas = new List<FaixasPontuacao>();

        if (currentActive != null && currentActive.FaixasPontuacao != null)
        {
            foreach (var partnerFaixa in currentActive.FaixasPontuacao.Where(f => f.Tipo == TipoFaixaPontuacaoEnum.Parceiro))
            {
                faixas.Add(new FaixasPontuacao
                {
                    RegraPontuacaoId = newConfig.Id,
                    Tipo = TipoFaixaPontuacaoEnum.Parceiro,
                    ValorVendas = partnerFaixa.ValorVendas,
                    Pontos = partnerFaixa.Pontos,
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
            PurchaseAmountPerPoint = newConfig.ValorCompraMinimoCliente,
            PointsGenerated = newConfig.PontosPorValorCliente,
            PointsForRedemption = newConfig.PontosParaConversaoMonetariaCliente,
            RedemptionValue = newConfig.ValorMonetarioPorPontosCliente,
            UpdatedAt = newConfig.CriadoEm
        };
    }
}
