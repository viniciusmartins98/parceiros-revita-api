using Mediator;
using RevitaParceiros.Application.Common.Interfaces;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.UpdatePartner;

public sealed class UpdatePartnerHandler(
    IParceiroRepository parceiroRepository,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<UpdatePartnerRequest, PartnerDto>
{
    public async ValueTask<PartnerDto> Handle(UpdatePartnerRequest request, CancellationToken cancellationToken)
    {
        var parceiro = await parceiroRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Parceiro não encontrado.");

        if (await parceiroRepository.ExistsByEmailExceptIdAsync(request.Email, request.Id, cancellationToken))
            throw new BusinessRuleException("Já existe outro usuário cadastrado com este e-mail.");
        if (await parceiroRepository.ExistsByCpfExceptIdAsync(request.Cpf, request.Id, cancellationToken))
            throw new BusinessRuleException("Já existe outro parceiro cadastrado com este cpf.");

        parceiro.Usuario.Nome = request.Name;
        parceiro.Usuario.Email = request.Email;
        parceiro.Usuario.Telefone = request.Phone;
        parceiro.Usuario.Ativo = request.IsActive;
        parceiro.Usuario.AtualizadoEm = dateTimeProvider.UtcNow;
        parceiro.AtualizadoEm = dateTimeProvider.UtcNow;

        await parceiroRepository.UpdateAsync(parceiro, cancellationToken);

        return new PartnerDto(
            parceiro.Id,
            parceiro.UsuarioId,
            parceiro.Usuario.Nome,
            parceiro.Usuario.Telefone,
            parceiro.Usuario.Email,
            parceiro.Cpf,
            parceiro.TotalPontos,
            parceiro.Usuario.Ativo,
            parceiro.Usuario.CriadoEm);
    }
}
