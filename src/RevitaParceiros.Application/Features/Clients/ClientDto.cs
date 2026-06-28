namespace RevitaParceiros.Application.Features.Clients;

public record ClientDto(
    Guid Id,
    Guid UserId,
    string Name,
    string Phone,
    string Email,
    int Points,
    bool IsActive,
    DateTime CreatedAt);
