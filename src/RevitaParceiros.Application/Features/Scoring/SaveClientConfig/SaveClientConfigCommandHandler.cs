using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.SaveClientConfig;

public sealed class SaveClientConfigCommandHandler : IRequestHandler<SaveClientConfigCommand, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;

    public SaveClientConfigCommandHandler(IRegrasPontuacaoRepository repository)
    {
        _repository = repository;
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
            ValorCompraMinimoParceiro = currentActive?.ValorCompraMinimoParceiro ?? 1000m,
            PontosPorValorParceiro = currentActive?.PontosPorValorParceiro ?? 100,
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

        await _repository.AddAsync(newConfig, cancellationToken);

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
