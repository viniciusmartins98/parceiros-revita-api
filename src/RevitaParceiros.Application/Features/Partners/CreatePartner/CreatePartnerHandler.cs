using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.CreatePartner;

public sealed class CreatePartnerHandler(
    IParceiroRepository parceiroRepository,
    IPasswordHasher passwordHasher,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<CreatePartnerRequest, PartnerDto>
{
    public async ValueTask<PartnerDto> Handle(CreatePartnerRequest request, CancellationToken cancellationToken)
    {
        if (await parceiroRepository.ExistsByEmailAsync(request.Email, cancellationToken))
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
            Perfil = PerfilUsuarioEnum.Parceiro,
            Parceiros = new Parceiros
            {
                Id = Guid.NewGuid(),
                TotalPontos = 0,
                CriadoEm = dateTimeProvider.UtcNow
            }
        };

        // Link the relationship
        usuario.Parceiros.UsuarioId = usuario.Id;

        await parceiroRepository.AddAsync(usuario, cancellationToken);

        return new PartnerDto(
            usuario.Id,
            usuario.Nome,
            usuario.Telefone,
            usuario.Email,
            usuario.Ativo,
            usuario.CriadoEm);
    }
}
