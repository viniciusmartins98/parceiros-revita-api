using Mediator;
using RevitaParceiros.Domain.Interfaces;
using System.Linq;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListEmployeePurchases;

public sealed class ListEmployeePurchasesHandler(ICompraFuncionarioRepository compraFuncionarioRepository)
    : IRequestHandler<ListEmployeePurchasesRequest, List<EmployeePurchaseDto>>
{
    public async ValueTask<List<EmployeePurchaseDto>> Handle(ListEmployeePurchasesRequest request, CancellationToken cancellationToken)
    {
        DateTime? startDate = request.Period.ToLower() switch
        {
            "week" => DateTime.UtcNow.AddDays(-7),
            "month" => DateTime.UtcNow.AddMonths(-1),
            "year" => DateTime.UtcNow.AddYears(-1),
            _ => null
        };

        var compras = await compraFuncionarioRepository.GetByFuncionarioIdAsync(request.FuncionarioId, startDate, cancellationToken);

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
