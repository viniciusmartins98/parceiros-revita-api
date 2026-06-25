using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Users.CreateUser;

public sealed class CreateUserHandler(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateUserCommand, UserDto>
{
    public async ValueTask<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await usuarioRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");
        }

        var passwordHash = passwordHasher.Hash(request.Password);

        var usuario = new Usuarios
        {
            Id = Guid.NewGuid(),
            Nome = request.Name,
            Email = request.Email,
            SenhaHash = passwordHash,
            Telefone = "", // Optional or not provided from UI currently
            Ativo = request.IsActive,
            CriadoEm = dateTimeProvider.UtcNow,
            Perfil = request.Role
        };

        switch (request.Role)
        {
            case PerfilUsuarioEnum.Administrador:
            case PerfilUsuarioEnum.Funcionario:
                usuario.Funcionarios = new Funcionarios
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuario.Id,
                    CriadoEm = dateTimeProvider.UtcNow
                };
                break;

            case PerfilUsuarioEnum.Cliente:
                usuario.Clientes = new Clientes
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuario.Id,
                    CriadoEm = dateTimeProvider.UtcNow
                };
                break;

            case PerfilUsuarioEnum.Parceiro:
                usuario.Parceiros = new Parceiros
                {
                    Id = Guid.NewGuid(),
                    UsuarioId = usuario.Id,
                    CriadoEm = dateTimeProvider.UtcNow
                };
                break;
        }

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return new UserDto(
            usuario.Id,
            usuario.Nome,
            usuario.Email,
            usuario.Telefone ?? "",
            usuario.Perfil,
            usuario.Ativo,
            usuario.CriadoEm
        );
    }
}
