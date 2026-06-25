using Mediator;
using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Application.Features.Users.CreateUser;

public sealed record CreateUserCommand(
    string Name,
    string Email,
    string Password,
    PerfilUsuarioEnum Role,
    bool IsActive
) : IRequest<UserDto>;
