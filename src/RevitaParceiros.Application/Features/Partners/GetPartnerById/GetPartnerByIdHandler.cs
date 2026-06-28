using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Partners.GetPartnerById;

public sealed class GetPartnerByIdHandler(IParceiroRepository parceiroRepository)
    : IRequestHandler<GetPartnerByIdRequest, PartnerDto>
{
    public async ValueTask<PartnerDto> Handle(GetPartnerByIdRequest request, CancellationToken cancellationToken)
    {
        var parceiro = await parceiroRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Parceiro não encontrado.");

        return new PartnerDto(
            parceiro.Id,
            parceiro.UsuarioId,
            parceiro.Usuario.Nome,
            parceiro.Usuario.Telefone,
            parceiro.Usuario.Email,
            parceiro.TotalPontos,
            parceiro.Usuario.Ativo,
            parceiro.Usuario.CriadoEm);
    }
}
