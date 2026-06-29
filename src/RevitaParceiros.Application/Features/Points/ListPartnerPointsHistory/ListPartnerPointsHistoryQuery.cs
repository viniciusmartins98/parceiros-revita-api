using Mediator;
using RevitaParceiros.Application.Features.Points;

namespace RevitaParceiros.Application.Features.Points.ListPartnerPointsHistory;

public record ListPartnerPointsHistoryQuery(Guid PartnerId) : IRequest<IReadOnlyCollection<PointHistoryDto>>;
