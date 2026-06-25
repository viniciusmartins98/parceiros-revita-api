using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.DeleteClient;

public sealed class DeleteClientHandler(IClienteRepository clienteRepository)
    : IRequestHandler<DeleteClientRequest, bool>
{
    public async ValueTask<bool> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

        await clienteRepository.DeleteAsync(cliente, cancellationToken);
        return true;
    }
}
