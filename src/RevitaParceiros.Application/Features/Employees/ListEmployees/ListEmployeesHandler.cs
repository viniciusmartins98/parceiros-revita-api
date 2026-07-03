using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.ListEmployees;

public sealed class ListEmployeesHandler(IFuncionarioRepository funcionarioRepository)
    : IRequestHandler<ListEmployeesRequest, IReadOnlyCollection<EmployeeDto>>
{
    public async ValueTask<IReadOnlyCollection<EmployeeDto>> Handle(ListEmployeesRequest request, CancellationToken cancellationToken)
    {
        var funcionarios = await funcionarioRepository.GetAllAsync(cancellationToken);

        return funcionarios.Select(f => new EmployeeDto(
            f.Id,
            f.UsuarioId,
            f.Usuario.Nome,
            f.Usuario.Telefone,
            f.Usuario.Email,
            f.Usuario.Ativo,
            f.Usuario.CriadoEm)).ToList();
    }
}
