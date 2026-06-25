using Mediator;

namespace RevitaParceiros.Application.Features.Clients.ListClients;

public record ListClientsRequest : IRequest<IReadOnlyCollection<ClientDto>>;
