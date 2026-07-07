using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Auth.RegistrarParceiro;

public sealed class RegistrarParceiroHandler(
    IUsuarioRepository usuarioRepository,
    IParceiroRepository parceiroRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider) : IRequestHandler<RegistrarParceiroRequest, RegistrarParceiroResponse>
{
    public async ValueTask<RegistrarParceiroResponse> Handle(
        RegistrarParceiroRequest request,
        CancellationToken cancellationToken)
    {
        var usuarioExistente = await usuarioRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (usuarioExistente is not null)
            throw new BusinessRuleException("Já existe um usuário cadastrado com este e-mail.");
        if (await parceiroRepository.ExistsByCpfAsync(request.Cpf, cancellationToken))
            throw new BusinessRuleException("Já existe um parceiro cadastrado com este cpf.");

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
            Perfil = PerfilUsuarioEnum.Parceiro,
            Parceiros = new Parceiros
            {
                Id = Guid.NewGuid(),
                TotalPontos = 0,
                Cpf = request.Cpf,
                CriadoEm = dateTimeProvider.UtcNow
            }
        };

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return new RegistrarParceiroResponse();
    }
}
