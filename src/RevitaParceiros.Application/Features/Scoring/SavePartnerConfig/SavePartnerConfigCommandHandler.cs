using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.SavePartnerConfig;

public sealed class SavePartnerConfigCommandHandler : IRequestHandler<SavePartnerConfigCommand, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;

    public SavePartnerConfigCommandHandler(IRegrasPontuacaoRepository repository)
    {
        _repository = repository;
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
            ValorCompraMinimoParceiro = request.PurchaseAmountPerPoint,
            PontosPorValorParceiro = request.PointsGenerated,
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

        await _repository.AddAsync(newConfig, cancellationToken);

        return new ScoringConfigDto
        {
            PurchaseAmountPerPoint = newConfig.ValorCompraMinimoParceiro,
            PointsGenerated = newConfig.PontosPorValorParceiro,
            PointsForRedemption = newConfig.PontosParaConversaoMonetariaParceiro,
            RedemptionValue = newConfig.ValorMonetarioPorPontosParceiro,
            UpdatedAt = newConfig.CriadoEm
        };
    }
}
