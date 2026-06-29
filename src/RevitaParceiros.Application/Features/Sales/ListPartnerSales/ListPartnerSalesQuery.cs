using Mediator;
using RevitaParceiros.Application.Features.Sales;

namespace RevitaParceiros.Application.Features.Sales.ListPartnerSales;

public record ListPartnerSalesQuery(Guid PartnerId) : IRequest<IReadOnlyCollection<SaleHistoryDto>>;
