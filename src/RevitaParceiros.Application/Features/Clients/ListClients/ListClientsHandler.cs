using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.ListClients;

public sealed class ListClientsHandler(IClienteRepository clienteRepository)
    : IRequestHandler<ListClientsRequest, IReadOnlyCollection<ClientDto>>
{
    public async ValueTask<IReadOnlyCollection<ClientDto>> Handle(ListClientsRequest request, CancellationToken cancellationToken)
    {
        var clientes = await clienteRepository.GetAllAsync(cancellationToken);

        return clientes.Select(c => new ClientDto(
            c.Id,
            c.UsuarioId,
            c.Usuario.Nome,
            c.Usuario.Telefone,
            c.Usuario.Email,
            c.Cpf,
            c.TotalPontos,
            c.Usuario.Ativo,
            c.Usuario.CriadoEm)).ToList();
    }
}
