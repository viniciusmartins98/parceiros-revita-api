using Mediator;

namespace RevitaParceiros.Application.Features.Sales.RegisterSale;

/// <summary>
/// Comando para registrar uma nova venda (compra) no sistema.
/// </summary>
public sealed record RegisterSaleCommand(
    decimal Amount,
    Guid PartnerId,
    Guid ClientId,
    Guid RegisteredByUserId
) : IRequest<RegisterSaleResponse>;
