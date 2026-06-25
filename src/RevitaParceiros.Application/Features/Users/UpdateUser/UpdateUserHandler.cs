using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Users.UpdateUser;

public sealed class UpdateUserHandler(
    IUsuarioRepository usuarioRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateUserCommand, UserDto>
{
    public async ValueTask<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        if (user.Email != request.Email)
        {
            if (await usuarioRepository.ExistsByEmailAsync(request.Email, cancellationToken))
            {
                throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");
            }
            user.Email = request.Email;
        }

        user.Nome = request.Name;
        user.Ativo = request.IsActive;
        user.AtualizadoEm = dateTimeProvider.UtcNow;
        
        var oldRole = user.Perfil;
        user.Perfil = request.Role;

        if (!string.IsNullOrEmpty(request.Password))
        {
            user.SenhaHash = passwordHasher.Hash(request.Password);
        }

        // Logic to add Funcionario record if role changed to Admin/Employee and didn't have it.
        // We will keep it simple for now, as usually role changing is complex.
        if (user.Funcionarios == null && (request.Role == PerfilUsuarioEnum.Administrador || request.Role == PerfilUsuarioEnum.Funcionario))
        {
             user.Funcionarios = new Funcionarios
             {
                 UsuarioId = user.Id,
                 CriadoEm = dateTimeProvider.UtcNow
             };
        }

        await usuarioRepository.UpdateAsync(user, cancellationToken);

        return new UserDto(
            user.Id,
            user.Nome,
            user.Email,
            user.Telefone ?? "",
            user.Perfil,
            user.Ativo,
            user.CriadoEm
        );
    }
}
