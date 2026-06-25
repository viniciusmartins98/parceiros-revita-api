using Mediator;

namespace RevitaParceiros.Application.Features.Users.GetUserById;

public sealed record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;
