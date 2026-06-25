using Mediator;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.ListPartners;

public sealed class ListPartnersHandler(IParceiroRepository parceiroRepository)
    : IRequestHandler<ListPartnersRequest, IReadOnlyCollection<PartnerDto>>
{
    public async ValueTask<IReadOnlyCollection<PartnerDto>> Handle(ListPartnersRequest request, CancellationToken cancellationToken)
    {
        var parceiros = await parceiroRepository.GetAllAsync(cancellationToken);

        return parceiros.Select(u => new PartnerDto(
            u.Id,
            u.Nome,
            u.Telefone,
            u.Email,
            u.Ativo,
            u.CriadoEm)).ToList();
    }
}
