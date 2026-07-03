using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.CreateEmployee;

public sealed class CreateEmployeeHandler(
    IFuncionarioRepository funcionarioRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateEmployeeRequest, EmployeeDto>
{
    public async ValueTask<EmployeeDto> Handle(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        if (await funcionarioRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");
        }

        var initialPassword = "Revita@" + request.Name.Split(' ').FirstOrDefault()?.Replace(" ", "");
        var passwordHash = passwordHasher.Hash(initialPassword);

        var funcionario = new Funcionarios
        {
            Id = Guid.NewGuid(),
            CriadoEm = dateTimeProvider.UtcNow,
            Usuario = new Usuarios
            {
                Id = Guid.NewGuid(),
                Nome = request.Name,
                Email = request.Email,
                Telefone = request.Phone,
                SenhaHash = passwordHash,
                Ativo = request.IsActive,
                CriadoEm = dateTimeProvider.UtcNow,
                Perfil = PerfilUsuarioEnum.Funcionario
            }
        };

        // Link the relationship
        funcionario.UsuarioId = funcionario.Usuario.Id;

        await funcionarioRepository.AddAsync(funcionario, cancellationToken);

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
