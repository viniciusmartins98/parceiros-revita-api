using Mediator;

namespace RevitaParceiros.Application.Features.Users.GetUsers;

public sealed record GetUsersQuery() : IRequest<List<UserDto>>;
