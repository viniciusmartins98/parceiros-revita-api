using Mediator;

namespace RevitaParceiros.Application.Features.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid Id) : IRequest;
