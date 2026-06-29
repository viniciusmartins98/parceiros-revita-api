using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;

namespace RevitaParceiros.Application.Features.Points.RedeemPoints;

public record RedeemClientPointsCommand(Guid ClientId, int Points, Guid LoggedUserId) : IRequest<RedeemPointsResponseDto>;
