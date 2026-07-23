using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Clients.GetClientProgress;

public sealed class GetClientProgressHandler(
    IClienteRepository clienteRepository,
    ICompraRepository compraRepository,
    IRegrasPontuacaoRepository regrasPontuacaoRepository)
    : IRequestHandler<GetClientProgressRequest, ClientProgressDto>
{
    public async ValueTask<ClientProgressDto> Handle(GetClientProgressRequest request, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException("Cliente não encontrado.");

        var regraAtiva = await regrasPontuacaoRepository.GetActiveConfigAsync(cancellationToken)
            ?? throw new BusinessRuleException("Não há regra de pontuação ativa configurada.");

        var (totalClientPurchases, _) = await compraRepository.GetClientAccumulatedAsync(cliente.Id, cancellationToken);
        
        var faturamentoResidual = totalClientPurchases % regraAtiva.ValorCompraMinimoCliente;

        return new ClientProgressDto(
            cliente.Id,
            cliente.TotalPontos,
            faturamentoResidual,
            regraAtiva.ValorCompraMinimoCliente,
            regraAtiva.PontosPorValorCliente
        );
    }
}
