using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;

namespace RevitaParceiros.Application.Features.Points.RedeemPoints;

public record RedeemPartnerPointsCommand(Guid PartnerId, int Points, Guid LoggedUserId) : IRequest<RedeemPointsResponseDto>;
