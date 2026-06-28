using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.GetClientByUserId;

public sealed class GetClientByUserIdHandler(IClienteRepository clienteRepository)
    : IRequestHandler<GetClientByUserIdRequest, ClientDto>
{
    public async ValueTask<ClientDto> Handle(GetClientByUserIdRequest request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.GetByUserIdAsync(request.Id, cancellationToken)
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
