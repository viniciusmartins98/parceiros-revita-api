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

        var usuario = new Usuarios
        {
            Id = Guid.NewGuid(),
            Nome = request.Name,
            Email = request.Email,
            Telefone = request.Phone,
            SenhaHash = passwordHash,
            Ativo = request.IsActive,
            CriadoEm = dateTimeProvider.UtcNow,
            Perfil = PerfilUsuarioEnum.Funcionario,
            Funcionarios = new Funcionarios
            {
                Id = Guid.NewGuid(),
                CriadoEm = dateTimeProvider.UtcNow
            }
        };

        // Link the relationship
        usuario.Funcionarios.UsuarioId = usuario.Id;

        await funcionarioRepository.AddAsync(usuario, cancellationToken);

        return new EmployeeDto(
            usuario.Id,
            usuario.Nome,
            usuario.Telefone,
            usuario.Email,
            usuario.Ativo,
            usuario.CriadoEm);
    }
}
