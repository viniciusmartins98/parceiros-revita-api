namespace RevitaParceiros.Application.Features.Clients;

public record ClientDto(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    bool IsActive,
    DateTime CreatedAt);
