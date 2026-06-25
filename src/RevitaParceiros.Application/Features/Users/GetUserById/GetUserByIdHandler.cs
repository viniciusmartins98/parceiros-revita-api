using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Users.GetUserById;

public sealed class GetUserByIdHandler(IUsuarioRepository usuarioRepository) : IRequestHandler<GetUserByIdQuery, UserDto>
{
    public async ValueTask<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

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
