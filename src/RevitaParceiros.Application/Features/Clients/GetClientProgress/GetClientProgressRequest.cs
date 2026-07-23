using Mediator;

namespace RevitaParceiros.Application.Features.Clients.GetClientProgress;

public sealed record GetClientProgressRequest(Guid Id) : IRequest<ClientProgressDto>;
