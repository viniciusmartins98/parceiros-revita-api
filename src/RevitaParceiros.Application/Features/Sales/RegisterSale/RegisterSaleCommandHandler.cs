using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Sales.RegisterSale;

/// <summary>
/// Handler responsável por registrar uma venda, calcular os pontos gerados
/// com base na regra de pontuação ativa e atualizar o saldo do parceiro.
/// </summary>
public sealed class RegisterSaleCommandHandler(
    ICompraRepository compraRepository,
    IRegrasPontuacaoRepository regrasPontuacaoRepository,
    IParceiroRepository parceiroRepository,
    IClienteRepository clienteRepository
) : IRequestHandler<RegisterSaleCommand, RegisterSaleResponse>
{
    public async ValueTask<RegisterSaleResponse> Handle(RegisterSaleCommand request, CancellationToken cancellationToken)
    {
        var parceiro = request.PartnerId.HasValue ? await parceiroRepository.GetByIdAsync(request.PartnerId.Value, cancellationToken) : null;

        var cliente = await clienteRepository.GetByIdAsync(request.ClientId, cancellationToken)
            ?? throw new NotFoundException("Cliente", request.ClientId);

        var regraAtiva = await regrasPontuacaoRepository.GetActiveConfigAsync(cancellationToken)
            ?? throw new BusinessRuleException("Não há regra de pontuação ativa configurada. Configure uma regra antes de registrar vendas.");

        // Calcular pontos Cliente de forma cumulativa
        var (totalClientPurchases, totalClientPoints) = await compraRepository.GetClientAccumulatedAsync(cliente.Id, cancellationToken);
        var newTotalClientPurchases = totalClientPurchases + request.Amount;
        var expectedTotalClientPoints = (int)Math.Floor(newTotalClientPurchases / regraAtiva.ValorCompraMinimoCliente) * regraAtiva.PontosPorValorCliente;
        var pontosGeradosCliente = Math.Max(0, expectedTotalClientPoints - totalClientPoints);

        // Calcular pontos Parceiro de forma cumulativa
        int pontosGeradosParceiro = 0;
        if (parceiro != null)
        {
            var (totalPartnerSales, totalPartnerPoints) = await compraRepository.GetPartnerAccumulatedAsync(parceiro.Id, cancellationToken);
            var newTotalPartnerSales = totalPartnerSales + request.Amount;
            var expectedTotalPartnerPoints = (int)Math.Floor(newTotalPartnerSales / regraAtiva.ValorCompraMinimoParceiro) * regraAtiva.PontosPorValorParceiro;
            pontosGeradosParceiro = Math.Max(0, expectedTotalPartnerPoints - totalPartnerPoints);
        }

        var compra = new Compras
        {
            Id = Guid.NewGuid(),
            ClienteId = cliente.Id,
            ParceiroId = parceiro?.Id,
            RegistradoPor = request.RegisteredByUserId,
            Valor = request.Amount,
            DataCompra = DateTime.UtcNow,
            PontosGeradosParceiro = pontosGeradosParceiro,
            PontosGeradosCliente = pontosGeradosCliente,
            CriadoEm = DateTime.UtcNow
        };

        var extratos = new List<ExtratoPontos>();

        if (pontosGeradosParceiro > 0)
        {
            extratos.Add(new ExtratoPontos
            {
                Id = Guid.NewGuid(),
                ParceiroId = parceiro.Id,
                ClienteId = null,
                TipoTransacao = TipoTransacaoPontosEnum.Ganho,
                Pontos = pontosGeradosParceiro,
                CompraId = compra.Id,
                Descricao = $"Pontos de indicação gerados pela venda de R$ {request.Amount:N2}",
                CriadoEm = DateTime.UtcNow
            });
            parceiro.TotalPontos += pontosGeradosParceiro;
        }

        if (pontosGeradosCliente > 0)
        {
            extratos.Add(new ExtratoPontos
            {
                Id = Guid.NewGuid(),
                ParceiroId = null,
                ClienteId = cliente.Id,
                TipoTransacao = TipoTransacaoPontosEnum.Ganho,
                Pontos = pontosGeradosCliente,
                CompraId = compra.Id,
                Descricao = $"Pontos de cashback gerados pela compra de R$ {request.Amount:N2}",
                CriadoEm = DateTime.UtcNow
            });
            cliente.TotalPontos += pontosGeradosCliente;
        }

        await compraRepository.RegisterSaleAsync(compra, extratos, cliente, parceiro, cancellationToken);

        return new RegisterSaleResponse(
            compra.Id,
            compra.Valor,
            request.PartnerId,
            request.ClientId,
            pontosGeradosParceiro + pontosGeradosCliente,
            compra.CriadoEm);
    }
}
