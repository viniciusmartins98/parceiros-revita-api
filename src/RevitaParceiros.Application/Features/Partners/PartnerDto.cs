namespace RevitaParceiros.Application.Features.Partners;

public record PartnerDto(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    bool IsActive,
    DateTime CreatedAt);
