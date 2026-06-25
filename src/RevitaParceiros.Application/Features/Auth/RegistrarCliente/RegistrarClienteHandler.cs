using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Auth.RegistrarCliente;

public sealed class RegistrarClienteHandler(
    IUsuarioRepository usuarioRepository,
    IClienteRepository clienteRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<RegistrarClienteRequest, RegistrarClienteResponse>
{
    public async ValueTask<RegistrarClienteResponse> Handle(
        RegistrarClienteRequest request,
        CancellationToken cancellationToken)
    {
        var usuarioExistente = await usuarioRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (usuarioExistente is not null)
            throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");

        var senhaHash = passwordHasher.Hash(request.Senha);

        var usuario = new Usuarios
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone,
            SenhaHash = senhaHash,
            Ativo = true,
            CriadoEm = dateTimeProvider.UtcNow,
            Perfil = PerfilUsuarioEnum.Cliente,
            Clientes = new Clientes
            {
                Id = Guid.NewGuid(),
                CriadoEm = dateTimeProvider.UtcNow
            }
        };

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return new RegistrarClienteResponse();
    }
}
