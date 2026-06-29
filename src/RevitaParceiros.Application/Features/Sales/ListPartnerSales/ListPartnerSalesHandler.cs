using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Sales.ListPartnerSales;

public sealed class ListPartnerSalesHandler(ICompraRepository compraRepository) : IRequestHandler<ListPartnerSalesQuery, IReadOnlyCollection<SaleHistoryDto>>
{
    public async ValueTask<IReadOnlyCollection<SaleHistoryDto>> Handle(ListPartnerSalesQuery request, CancellationToken cancellationToken)
    {
        var compras = await compraRepository.GetByPartnerIdAsync(request.PartnerId, cancellationToken);

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
