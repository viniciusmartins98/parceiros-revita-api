using Mediator;

namespace RevitaParceiros.Application.Features.Clients.CreateClient;

public record CreateClientRequest(
    string Name,
    string Phone,
    string Email,
    bool IsActive) : IRequest<ClientDto>;
