using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Points.ListPartnerPointsHistory;

public sealed class ListPartnerPointsHistoryHandler(IExtratoPontosRepository extratoPontosRepository) : IRequestHandler<ListPartnerPointsHistoryQuery, IReadOnlyCollection<PointHistoryDto>>
{
    public async ValueTask<IReadOnlyCollection<PointHistoryDto>> Handle(ListPartnerPointsHistoryQuery request, CancellationToken cancellationToken)
    {
        var historico = await extratoPontosRepository.GetByPartnerIdAsync(request.PartnerId, cancellationToken);

        return historico.Take(100).Select(e => new PointHistoryDto(
            e.Id,
            e.Pontos,
            e.ValorMonetario,
            e.Descricao,
            e.CriadoEm,
            e.ParceiroId,
            e.ClienteId
        )).ToList();
    }
}
