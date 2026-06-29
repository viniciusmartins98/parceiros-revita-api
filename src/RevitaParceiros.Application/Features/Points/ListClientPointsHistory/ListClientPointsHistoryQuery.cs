using Mediator;
using RevitaParceiros.Application.Features.Points;

namespace RevitaParceiros.Application.Features.Points.ListClientPointsHistory;

public record ListClientPointsHistoryQuery(Guid ClientId) : IRequest<IReadOnlyCollection<PointHistoryDto>>;
