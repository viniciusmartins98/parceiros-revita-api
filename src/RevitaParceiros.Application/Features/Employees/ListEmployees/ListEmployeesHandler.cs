using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.ListEmployees;

public sealed class ListEmployeesHandler(IFuncionarioRepository funcionarioRepository)
    : IRequestHandler<ListEmployeesRequest, IReadOnlyCollection<EmployeeDto>>
{
    public async ValueTask<IReadOnlyCollection<EmployeeDto>> Handle(ListEmployeesRequest request, CancellationToken cancellationToken)
    {
        var usuarios = await funcionarioRepository.GetAllAsync(cancellationToken);

        return usuarios.Select(u => new EmployeeDto(
            u.Id,
            u.Nome,
            u.Telefone,
            u.Email,
            u.Ativo,
            u.CriadoEm)).ToList();
    }
}
