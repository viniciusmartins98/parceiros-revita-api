using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.GetEmployeeById;

public sealed class GetEmployeeByIdHandler(IFuncionarioRepository funcionarioRepository)
    : IRequestHandler<GetEmployeeByIdRequest, EmployeeDto>
{
    public async ValueTask<EmployeeDto> Handle(GetEmployeeByIdRequest request, CancellationToken cancellationToken)
    {
        var funcionario = await funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (funcionario is null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        return new EmployeeDto(
            funcionario.Id,
            funcionario.UsuarioId,
            funcionario.Usuario.Nome,
            funcionario.Usuario.Telefone,
            funcionario.Usuario.Email,
            funcionario.Usuario.Ativo,
            funcionario.Usuario.CriadoEm);
    }
}
