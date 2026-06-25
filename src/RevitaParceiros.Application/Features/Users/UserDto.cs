using RevitaParceiros.Domain.Enums;

namespace RevitaParceiros.Application.Features.Users;

public sealed record UserDto(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    PerfilUsuarioEnum Role,
    bool IsActive,
    DateTime CreatedAt
);
