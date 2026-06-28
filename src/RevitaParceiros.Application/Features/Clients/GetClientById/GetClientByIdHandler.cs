using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.GetClientById;

public sealed class GetClientByIdHandler(IClienteRepository clienteRepository)
    : IRequestHandler<GetClientByIdRequest, ClientDto>
{
    public async ValueTask<ClientDto> Handle(GetClientByIdRequest request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

        return new ClientDto(
            cliente.Id,
            cliente.UsuarioId,
            cliente.Usuario.Nome,
            cliente.Usuario.Telefone,
            cliente.Usuario.Email,
            cliente.TotalPontos,
            cliente.Usuario.Ativo,
            cliente.Usuario.CriadoEm);
    }
}
