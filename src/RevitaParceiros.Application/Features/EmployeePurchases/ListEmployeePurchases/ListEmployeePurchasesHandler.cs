using Mediator;
using RevitaParceiros.Domain.Interfaces;
using System.Linq;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListEmployeePurchases;

public sealed class ListEmployeePurchasesHandler(ICompraFuncionarioRepository compraFuncionarioRepository)
    : IRequestHandler<ListEmployeePurchasesRequest, List<EmployeePurchaseDto>>
{
    public async ValueTask<List<EmployeePurchaseDto>> Handle(ListEmployeePurchasesRequest request, CancellationToken cancellationToken)
    {
        var compras = await compraFuncionarioRepository.GetByFuncionarioIdAsync(request.FuncionarioId, cancellationToken);

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
