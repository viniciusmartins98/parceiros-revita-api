using Mediator;
using RevitaParceiros.Domain.Interfaces;
using System.Linq;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;

public sealed class ListAllEmployeePurchasesHandler(ICompraFuncionarioRepository compraFuncionarioRepository)
    : IRequestHandler<ListAllEmployeePurchasesRequest, List<EmployeePurchaseDto>>
{
    public async ValueTask<List<EmployeePurchaseDto>> Handle(ListAllEmployeePurchasesRequest request, CancellationToken cancellationToken)
    {
        var compras = await compraFuncionarioRepository.GetAllAsync(cancellationToken);

        return compras.Select(c => new EmployeePurchaseDto(
            c.Id,
            c.Valor,
            c.Descricao,
            c.DataCompra,
            c.Funcionario?.Usuario?.Nome ?? string.Empty,
            c.CriadoEm
        )).ToList();
    }
}
