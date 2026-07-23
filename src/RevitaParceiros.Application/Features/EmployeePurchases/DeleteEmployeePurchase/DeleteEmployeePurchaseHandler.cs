using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.EmployeePurchases.DeleteEmployeePurchase;

public sealed class DeleteEmployeePurchaseHandler(ICompraFuncionarioRepository compraFuncionarioRepository)
    : IRequestHandler<DeleteEmployeePurchaseCommand>
{
    public async ValueTask<Unit> Handle(DeleteEmployeePurchaseCommand request, CancellationToken cancellationToken)
    {
        await compraFuncionarioRepository.DeleteAsync(request.Id, cancellationToken);
        return Unit.Value;
    }
}
