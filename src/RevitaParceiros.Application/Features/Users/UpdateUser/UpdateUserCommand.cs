using Mediator;
using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Application.Features.Users.UpdateUser;

public sealed record UpdateUserCommand(
    Guid Id,
    string Name,
    string Email,
    string? Password,
    PerfilUsuarioEnum Role,
    bool IsActive
) : IRequest<UserDto>;
