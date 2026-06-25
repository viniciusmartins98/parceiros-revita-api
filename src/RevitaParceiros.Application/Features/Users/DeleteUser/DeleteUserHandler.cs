using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Users.DeleteUser;

public sealed class DeleteUserHandler(IUsuarioRepository usuarioRepository) : IRequestHandler<DeleteUserCommand>
{
    public async ValueTask<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await usuarioRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Usuário não encontrado.");

        await usuarioRepository.DeleteAsync(user, cancellationToken);

        return Unit.Value;
    }
}
