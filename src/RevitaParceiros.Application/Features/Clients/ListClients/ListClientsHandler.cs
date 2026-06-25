using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.ListClients;

public sealed class ListClientsHandler(IClienteRepository clienteRepository)
    : IRequestHandler<ListClientsRequest, IReadOnlyCollection<ClientDto>>
{
    public async ValueTask<IReadOnlyCollection<ClientDto>> Handle(ListClientsRequest request, CancellationToken cancellationToken)
    {
        var clientes = await clienteRepository.GetAllAsync(cancellationToken);

        return clientes.Select(u => new ClientDto(
            u.Id,
            u.Nome,
            u.Telefone,
            u.Email,
            u.Ativo,
            u.CriadoEm)).ToList();
    }
}
