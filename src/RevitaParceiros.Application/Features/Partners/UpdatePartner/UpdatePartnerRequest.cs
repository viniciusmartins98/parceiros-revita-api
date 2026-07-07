using Mediator;
using System.Text.Json.Serialization;

namespace RevitaParceiros.Application.Features.Partners.UpdatePartner;

public record UpdatePartnerRequest(
    [property: JsonIgnore] Guid Id,
    string Name,
    string Phone,
    string Email,
    string Cpf,
    bool IsActive) : IRequest<PartnerDto>;
