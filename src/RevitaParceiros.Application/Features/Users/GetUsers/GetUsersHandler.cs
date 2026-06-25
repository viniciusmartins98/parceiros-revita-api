using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Users.GetUsers;

public sealed class GetUsersHandler(IUsuarioRepository usuarioRepository) : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    public async ValueTask<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await usuarioRepository.GetAllAsync(cancellationToken);

        return users.Select(u => new UserDto(
            u.Id,
            u.Nome,
            u.Email,
            u.Telefone ?? "",
            u.Perfil,
            u.Ativo,
            u.CriadoEm
        )).ToList();
    }
}
