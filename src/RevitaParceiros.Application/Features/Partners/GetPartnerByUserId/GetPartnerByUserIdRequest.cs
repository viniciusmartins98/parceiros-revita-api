using Mediator;
using RevitaParceiros.Application.Features.Partners;

namespace RevitaParceiros.Application.Features.Clients.GetPartnerByUserId;

public record GetPartnerByUserIdRequest(Guid Id) : IRequest<PartnerDto>;
