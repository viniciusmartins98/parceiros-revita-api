using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.DeleteEmployee;

public sealed class DeleteEmployeeHandler(IFuncionarioRepository funcionarioRepository)
    : IRequestHandler<DeleteEmployeeRequest>
{
    public async ValueTask<Unit> Handle(DeleteEmployeeRequest request, CancellationToken cancellationToken)
    {
        var usuario = await funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario is null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        await funcionarioRepository.DeleteAsync(usuario, cancellationToken);

        return Unit.Value;
    }
}
