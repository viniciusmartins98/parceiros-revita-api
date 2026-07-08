namespace RevitaParceiros.Application.Features.Sales.RegisterSale;

/// <summary>
/// Resposta retornada após registrar uma venda com sucesso.
/// </summary>
public sealed record RegisterSaleResponse(
    Guid Id,
    decimal Amount,
    Guid? PartnerId,
    Guid ClientId,
    int PointsGenerated,
    DateTime CreatedAt);
