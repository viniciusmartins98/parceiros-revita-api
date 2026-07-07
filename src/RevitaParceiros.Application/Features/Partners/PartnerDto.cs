namespace RevitaParceiros.Application.Features.Partners;

public record PartnerDto(
    Guid Id,
    Guid UserId,
    string Name,
    string Phone,
    string Email,
    string Cpf,
    int Points,
    bool IsActive,
    DateTime CreatedAt);
