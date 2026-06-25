using Mediator;

namespace RevitaParceiros.Application.Features.Clients.GetClientById;

public record GetClientByIdRequest(Guid Id) : IRequest<ClientDto>;
