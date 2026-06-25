using Mediator;

namespace RevitaParceiros.Application.Features.Partners.DeletePartner;

public record DeletePartnerRequest(Guid Id) : IRequest<bool>;
