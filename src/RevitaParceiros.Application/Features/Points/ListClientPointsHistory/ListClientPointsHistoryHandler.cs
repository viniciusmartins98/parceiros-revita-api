using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Points.ListClientPointsHistory;

public sealed class ListClientPointsHistoryHandler(IExtratoPontosRepository extratoPontosRepository) : IRequestHandler<ListClientPointsHistoryQuery, IReadOnlyCollection<PointHistoryDto>>
{
    public async ValueTask<IReadOnlyCollection<PointHistoryDto>> Handle(ListClientPointsHistoryQuery request, CancellationToken cancellationToken)
    {
        var historico = await extratoPontosRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

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
