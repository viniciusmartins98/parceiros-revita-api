namespace RevitaParceiros.Application.Features.Points;

public record PointHistoryDto(
    Guid Id,
    int Points,
    decimal? MonetaryValue,
    string Description,
    DateTime CreatedAt,
    Guid? PartnerId,
    Guid? ClientId
);
