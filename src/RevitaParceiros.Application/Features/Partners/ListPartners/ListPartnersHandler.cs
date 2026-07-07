using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.ListPartners;

public sealed class ListPartnersHandler(IParceiroRepository parceiroRepository)
    : IRequestHandler<ListPartnersRequest, IReadOnlyCollection<PartnerDto>>
{
    public async ValueTask<IReadOnlyCollection<PartnerDto>> Handle(ListPartnersRequest request, CancellationToken cancellationToken)
    {
        var parceiros = await parceiroRepository.GetAllAsync(cancellationToken);

        return parceiros.Select(p => new PartnerDto(
            p.Id,
            p.UsuarioId,
            p.Usuario.Nome,
            p.Usuario.Telefone,
            p.Usuario.Email,
            p.Cpf,
            p.TotalPontos,
            p.Usuario.Ativo,
            p.Usuario.CriadoEm)).ToList();
    }
}
