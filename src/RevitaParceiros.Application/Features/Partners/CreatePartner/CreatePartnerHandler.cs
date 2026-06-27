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

        var parceiro = new Parceiros
        {
            Id = Guid.NewGuid(),
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
                Perfil = PerfilUsuarioEnum.Parceiro
            }
        };

        // Link the relationship
        parceiro.UsuarioId = parceiro.Usuario.Id;

        await parceiroRepository.AddAsync(parceiro, cancellationToken);

        return new PartnerDto(
            parceiro.Id,
            parceiro.Usuario.Nome,
            parceiro.Usuario.Telefone,
            parceiro.Usuario.Email,
            parceiro.Usuario.Ativo,
            parceiro.Usuario.CriadoEm);
    }
}
