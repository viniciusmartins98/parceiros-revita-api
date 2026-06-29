using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;
using System.Collections.Generic;

namespace RevitaParceiros.Application.Features.Points.GetRecentRedemptions;

public record GetRecentRedemptionsQuery(int Limit = 50) : IRequest<IEnumerable<RecentRedemptionDto>>;
