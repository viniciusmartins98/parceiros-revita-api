using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Sales.ListClientSales;

public sealed class ListClientSalesHandler(ICompraRepository compraRepository) : IRequestHandler<ListClientSalesQuery, IReadOnlyCollection<SaleHistoryDto>>
{
    public async ValueTask<IReadOnlyCollection<SaleHistoryDto>> Handle(ListClientSalesQuery request, CancellationToken cancellationToken)
    {
        var compras = await compraRepository.GetByClientIdAsync(request.ClientId, cancellationToken);

        return compras.Take(100).Select(c => new SaleHistoryDto(
            c.Id,
            c.Valor,
            c.ParceiroId,
            c.Parceiro.Usuario.Nome,
            c.ClienteId,
            c.Cliente.Usuario.Nome,
            c.DataCompra,
            c.PontosGeradosCliente,
            c.PontosGeradosParceiro
        )).ToList();
    }
}
