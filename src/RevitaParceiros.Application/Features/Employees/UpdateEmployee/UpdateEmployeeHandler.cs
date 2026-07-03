using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.UpdateEmployee;

public sealed class UpdateEmployeeHandler(
    IFuncionarioRepository funcionarioRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateEmployeeRequest, EmployeeDto>
{
    public async ValueTask<EmployeeDto> Handle(UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var funcionario = await funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (funcionario is null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        if (await funcionarioRepository.ExistsByEmailExceptIdAsync(request.Email, request.Id, cancellationToken))
        {
            throw new BusinessRuleException("Já existe outro usuário cadastrado com este e-mail.");
        }

        funcionario.Usuario.Nome = request.Name;
        funcionario.Usuario.Email = request.Email;
        funcionario.Usuario.Telefone = request.Phone;
        funcionario.Usuario.Ativo = request.IsActive;
        funcionario.Usuario.AtualizadoEm = dateTimeProvider.UtcNow;
        funcionario.AtualizadoEm = dateTimeProvider.UtcNow;

        await funcionarioRepository.UpdateAsync(funcionario, cancellationToken);

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
