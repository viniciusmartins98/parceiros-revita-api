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

        cliente.Usuario.Nome = request.Name;
        cliente.Usuario.Email = request.Email;
        cliente.Usuario.Telefone = request.Phone;
        cliente.Usuario.Ativo = request.IsActive;
        cliente.Usuario.AtualizadoEm = dateTimeProvider.UtcNow;
        cliente.AtualizadoEm = dateTimeProvider.UtcNow;

        await clienteRepository.UpdateAsync(cliente, cancellationToken);

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
