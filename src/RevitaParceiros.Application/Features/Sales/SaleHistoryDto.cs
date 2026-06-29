namespace RevitaParceiros.Application.Features.Sales;

public record SaleHistoryDto(
    Guid Id,
    decimal Amount,
    Guid PartnerId,
    string PartnerName,
    Guid ClientId,
    string ClientName,
    DateTime CreatedAt,
    int PointsGeneratedClient,
    int PointsGeneratedPartner
);
