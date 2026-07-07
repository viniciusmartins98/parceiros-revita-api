using Mediator;
using System.Text.Json.Serialization;

namespace RevitaParceiros.Application.Features.Clients.UpdateClient;

public record UpdateClientRequest(
    [property: JsonIgnore] Guid Id,
    string Name,
    string Phone,
    string Email,
    string Cpf,
    bool IsActive) : IRequest<ClientDto>;
