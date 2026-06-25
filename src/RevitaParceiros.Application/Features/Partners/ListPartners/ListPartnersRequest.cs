using Mediator;

namespace RevitaParceiros.Application.Features.Partners.ListPartners;

public record ListPartnersRequest : IRequest<IReadOnlyCollection<PartnerDto>>;
