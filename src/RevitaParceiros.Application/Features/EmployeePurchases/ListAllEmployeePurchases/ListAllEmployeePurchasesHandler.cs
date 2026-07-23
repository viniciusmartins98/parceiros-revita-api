using Mediator;
using RevitaParceiros.Domain.Interfaces;
using System.Linq;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;

public sealed class ListAllEmployeePurchasesHandler(ICompraFuncionarioRepository compraFuncionarioRepository)
    : IRequestHandler<ListAllEmployeePurchasesRequest, List<EmployeePurchaseDto>>
{
    public async ValueTask<List<EmployeePurchaseDto>> Handle(ListAllEmployeePurchasesRequest request, CancellationToken cancellationToken)
    {
        DateTime? startDate = request.StartDate;
        DateTime? endDate = request.EndDate;

        if (!startDate.HasValue && !endDate.HasValue && !string.IsNullOrEmpty(request.Period))
        {
            startDate = request.Period.ToLower() switch
            {
                "week" => DateTime.UtcNow.AddDays(-7),
                "month" => DateTime.UtcNow.AddMonths(-1),
                "year" => DateTime.UtcNow.AddYears(-1),
                _ => null
            };
        }

        var compras = await compraFuncionarioRepository.GetAllAsync(startDate, endDate, cancellationToken);

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
