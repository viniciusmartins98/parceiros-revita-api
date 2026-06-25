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
        var usuario = await funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (usuario is null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        if (await funcionarioRepository.ExistsByEmailExceptIdAsync(request.Email, request.Id, cancellationToken))
        {
            throw new BusinessRuleException("Já existe outro usuário cadastrado com este e-mail.");
        }

        usuario.Nome = request.Name;
        usuario.Email = request.Email;
        usuario.Telefone = request.Phone;
        usuario.Ativo = request.IsActive;
        usuario.AtualizadoEm = dateTimeProvider.UtcNow;

        if (usuario.Funcionarios != null)
        {
            usuario.Funcionarios.AtualizadoEm = dateTimeProvider.UtcNow;
        }

        await funcionarioRepository.UpdateAsync(usuario, cancellationToken);

        return new EmployeeDto(
            usuario.Id,
            usuario.Nome,
            usuario.Telefone,
            usuario.Email,
            usuario.Ativo,
            usuario.CriadoEm);
    }
}
