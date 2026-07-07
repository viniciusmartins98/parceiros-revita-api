using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.CreateClient;

public sealed class CreateClientHandler(
    IClienteRepository clienteRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreateClientRequest, ClientDto>
{
    public async ValueTask<ClientDto> Handle(CreateClientRequest request, CancellationToken cancellationToken)
    {
        if (await clienteRepository.ExistsByEmailAsync(request.Email, cancellationToken))
        {
            throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");
        }
        if (await clienteRepository.ExistsByCpfAsync(request.Cpf, cancellationToken))
        {
            throw new BusinessRuleException("Já existe um cliente cadastrado com este cpf.");
        }

        var initialPassword = "Revita@" + request.Name.Split(' ').FirstOrDefault()?.Replace(" ", "");
        var passwordHash = passwordHasher.Hash(initialPassword);

        var cliente = new Clientes
        {
            Id = Guid.NewGuid(),
            Cpf = request.Cpf,
            TotalPontos = 0,
            CriadoEm = dateTimeProvider.UtcNow,
            Usuario = new Usuarios
            {
                Id = Guid.NewGuid(),
                Nome = request.Name,
                Email = request.Email,
                Telefone = request.Phone,
                SenhaHash = passwordHash,
                Ativo = request.IsActive,
                CriadoEm = dateTimeProvider.UtcNow,
                Perfil = PerfilUsuarioEnum.Cliente
            }
        };

        // Link the relationship
        cliente.UsuarioId = cliente.Usuario.Id;

        await clienteRepository.AddAsync(cliente, cancellationToken);

        return new ClientDto(
            cliente.Id,
            cliente.UsuarioId,
            cliente.Usuario.Nome,
            cliente.Usuario.Telefone,
            cliente.Usuario.Email,
            cliente.Cpf,
            cliente.TotalPontos,
            cliente.Usuario.Ativo,
            cliente.Usuario.CriadoEm);
    }
}
