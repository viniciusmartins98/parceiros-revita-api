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

        var initialPassword = "Revita@" + request.Name.Split(' ').FirstOrDefault()?.Replace(" ", "");
        var passwordHash = passwordHasher.Hash(initialPassword);

        var usuario = new Usuarios
        {
            Id = Guid.NewGuid(),
            Nome = request.Name,
            Email = request.Email,
            Telefone = request.Phone,
            SenhaHash = passwordHash,
            Ativo = request.IsActive,
            CriadoEm = dateTimeProvider.UtcNow,
            Perfil = PerfilUsuarioEnum.Cliente,
            Clientes = new Clientes
            {
                Id = Guid.NewGuid(),
                CriadoEm = dateTimeProvider.UtcNow
            }
        };

        // Link the relationship
        usuario.Clientes.UsuarioId = usuario.Id;

        await clienteRepository.AddAsync(usuario, cancellationToken);

        return new ClientDto(
            usuario.Id,
            usuario.Nome,
            usuario.Telefone,
            usuario.Email,
            usuario.Ativo,
            usuario.CriadoEm);
    }
}
