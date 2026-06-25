using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.DeletePartner;

public sealed class DeletePartnerHandler(IParceiroRepository parceiroRepository)
    : IRequestHandler<DeletePartnerRequest, bool>
{
    public async ValueTask<bool> Handle(DeletePartnerRequest request, CancellationToken cancellationToken)
    {
        var parceiro = await parceiroRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Parceiro não encontrado.");

        await parceiroRepository.DeleteAsync(parceiro, cancellationToken);
        return true;
    }
}
