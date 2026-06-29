using Mediator;
using RevitaParceiros.Application.Features.Sales;

namespace RevitaParceiros.Application.Features.Sales.ListClientSales;

public record ListClientSalesQuery(Guid ClientId) : IRequest<IReadOnlyCollection<SaleHistoryDto>>;
