using Mediator;

namespace RevitaParceiros.Application.Features.Partners.GetPartnerById;

public record GetPartnerByIdRequest(Guid Id) : IRequest<PartnerDto>;
