using Mediator;

namespace RevitaParceiros.Application.Features.Partners.CreatePartner;

public record CreatePartnerRequest(
    string Name,
    string Phone,
    string Email,
    bool IsActive) : IRequest<PartnerDto>;
