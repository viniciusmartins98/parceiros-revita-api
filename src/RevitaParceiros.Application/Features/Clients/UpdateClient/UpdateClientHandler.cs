using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.UpdateClient;

public sealed class UpdateClientHandler(
    IClienteRepository clienteRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdateClientRequest, ClientDto>
{
    public async ValueTask<ClientDto> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

        if (await clienteRepository.ExistsByEmailExceptIdAsync(request.Email, request.Id, cancellationToken))
        {
            throw new BusinessRuleException("Já existe outro usuário cadastrado com este e-mail.");
        }

        cliente.Nome = request.Name;
        cliente.Email = request.Email;
        cliente.Telefone = request.Phone;
        cliente.Ativo = request.IsActive;
        cliente.AtualizadoEm = dateTimeProvider.UtcNow;
        if (cliente.Clientes != null)
        {
            cliente.Clientes.AtualizadoEm = dateTimeProvider.UtcNow;
        }

        await clienteRepository.UpdateAsync(cliente, cancellationToken);

        return new ClientDto(
            cliente.Id,
            cliente.Nome,
            cliente.Telefone,
            cliente.Email,
            cliente.Ativo,
            cliente.CriadoEm);
    }
}
