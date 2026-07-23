using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.GetPartnerProgress;

public sealed class GetPartnerProgressHandler(
    IParceiroRepository parceiroRepository,
    ICompraRepository compraRepository,
    IExtratoPontosRepository extratoPontosRepository,
    IRegrasPontuacaoRepository scoreConfigRepository)
    : IRequestHandler<GetPartnerProgressRequest, PartnerProgressDto>
{
    public async ValueTask<PartnerProgressDto> Handle(GetPartnerProgressRequest request, CancellationToken cancellationToken)
    {
        var parceiro = await parceiroRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Parceiro não encontrado.");

        var lastRedemptionDate = await extratoPontosRepository.GetPartnerLastRedemptionDateAsync(request.Id, cancellationToken);
        var (totalPartnerSales, _) = await compraRepository.GetPartnerAccumulatedAsync(request.Id, lastRedemptionDate, cancellationToken);

        var scoreConfig = await scoreConfigRepository.GetActiveConfigAsync();
        var faixasDb = scoreConfig.FaixasPontuacao ?? [];
        var faixas = new List<FaixasPontuacaoDto>();
        var pontos = 0;
        foreach (var item in faixasDb)
        {
            pontos += item.Pontos;
            faixas.Add(new FaixasPontuacaoDto(item.ValorVendas, pontos));
        }

        return new PartnerProgressDto(
            parceiro.Id,
            parceiro.TotalPontos,
            totalPartnerSales,
            faixas
        );
    }
}
