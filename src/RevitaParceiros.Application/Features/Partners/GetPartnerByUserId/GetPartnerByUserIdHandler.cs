using Mediator;
using RevitaParceiros.Application.Features.Partners;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.GetPartnerByUserId;

public sealed class GetPartnerByUserIdHandler(IParceiroRepository parceiroRepository)
    : IRequestHandler<GetPartnerByUserIdRequest, PartnerDto>
{
    public async ValueTask<PartnerDto> Handle(GetPartnerByUserIdRequest request, CancellationToken cancellationToken)
    {
        var parceiro = await parceiroRepository.GetByUserIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

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
