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
        {
            throw new BusinessRuleException("Já existe outro usuário cadastrado com este e-mail.");
        }

        parceiro.Nome = request.Name;
        parceiro.Email = request.Email;
        parceiro.Telefone = request.Phone;
        parceiro.Ativo = request.IsActive;
        parceiro.AtualizadoEm = dateTimeProvider.UtcNow;
        if (parceiro.Parceiros != null)
        {
            parceiro.Parceiros.AtualizadoEm = dateTimeProvider.UtcNow;
        }

        await parceiroRepository.UpdateAsync(parceiro, cancellationToken);

        return new PartnerDto(
            parceiro.Id,
            parceiro.Nome,
            parceiro.Telefone,
            parceiro.Email,
            parceiro.Ativo,
            parceiro.CriadoEm);
    }
}
