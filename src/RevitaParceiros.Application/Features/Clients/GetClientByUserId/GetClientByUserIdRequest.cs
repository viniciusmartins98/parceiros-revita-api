using Mediator;

namespace RevitaParceiros.Application.Features.Clients.GetClientByUserId;

public record GetClientByUserIdRequest(Guid Id) : IRequest<ClientDto>;
