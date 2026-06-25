using Mediator;

namespace RevitaParceiros.Application.Features.Clients.DeleteClient;

public record DeleteClientRequest(Guid Id) : IRequest<bool>;
