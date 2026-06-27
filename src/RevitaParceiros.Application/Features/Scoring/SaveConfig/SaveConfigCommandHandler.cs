using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Scoring.SaveConfig;

public sealed class SaveConfigCommandHandler : IRequestHandler<SaveConfigCommand, ScoringConfigDto>
{
    private readonly IRegrasPontuacaoRepository _repository;

    public SaveConfigCommandHandler(IRegrasPontuacaoRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<ScoringConfigDto> Handle(SaveConfigCommand request, CancellationToken cancellationToken)
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
            ValorCompraMinimo = request.PurchaseAmountPerPoint,
            PontosPorValor = request.PointsGenerated,
            PontosParaConversaoMonetaria = request.PointsForRedemption,
            ValorMonetarioPorPontos = request.RedemptionValue,
            Ativo = true,
            CriadoPor = request.UserId,
            CriadoEm = DateTime.UtcNow
        };

        await _repository.AddAsync(newConfig, cancellationToken);

        return new ScoringConfigDto
        {
            PurchaseAmountPerPoint = newConfig.ValorCompraMinimo,
            PointsGenerated = newConfig.PontosPorValor,
            PointsForRedemption = newConfig.PontosParaConversaoMonetaria,
            RedemptionValue = newConfig.ValorMonetarioPorPontos,
            UpdatedAt = newConfig.CriadoEm
        };
    }
}
