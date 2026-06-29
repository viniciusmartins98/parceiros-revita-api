using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Points.RedeemPoints;

public class RedeemClientPointsCommandHandler(
    IClienteRepository clienteRepository,
    IExtratoPontosRepository extratoPontosRepository,
    IResgatesRepository resgatesRepository,
    IRegrasPontuacaoRepository regrasPontuacaoRepository) : IRequestHandler<RedeemClientPointsCommand, RedeemPointsResponseDto>
{
    public async ValueTask<RedeemPointsResponseDto> Handle(RedeemClientPointsCommand request, CancellationToken cancellationToken)
    {
        var config = await regrasPontuacaoRepository.GetActiveConfigAsync(cancellationToken);
        if (config == null)
            throw new BusinessRuleException("Nenhuma regra de pontuação ativa encontrada.");

        var cliente = await clienteRepository.GetByIdAsync(request.ClientId, cancellationToken);
        if (cliente == null)
            throw new BusinessRuleException("Cliente não encontrado.");

        if (request.Points > cliente.TotalPontos)
            throw new BusinessRuleException("Pontos insuficientes para resgate.");

        if (request.Points % config.PontosParaConversaoMonetariaCliente != 0)
            throw new BusinessRuleException($"Os pontos devem ser múltiplos de {config.PontosParaConversaoMonetariaCliente}.");

        decimal valorMonetario = (request.Points / config.PontosParaConversaoMonetariaCliente) * config.ValorMonetarioPorPontosCliente;

        cliente.TotalPontos -= request.Points;

        var extrato = new ExtratoPontos
        {
            ClienteId = cliente.Id,
            Pontos = -request.Points,
            ValorMonetario = valorMonetario,
            Descricao = $"Resgate de {request.Points} pontos concedendo um desconto de R$ {valorMonetario:F2} para o cliente.",
            TipoTransacao = TipoTransacaoPontosEnum.Resgate,
            CriadoEm = DateTime.UtcNow
        };

        var resgate = new Resgates
        {
            ClienteId = cliente.Id,
            PontosResgatados = request.Points,
            ValorMonetario = valorMonetario,
            AprovadoPor = request.LoggedUserId,
            Status = StatusResgateEnum.Aprovado,
            Observacoes = "Resgate processado automaticamente para o cliente",
            CriadoEm = DateTime.UtcNow
        };

        await extratoPontosRepository.AddAsync(extrato, cancellationToken);
        await resgatesRepository.AddAsync(resgate, cancellationToken);
        await clienteRepository.UpdateAsync(cliente, cancellationToken);

        return new RedeemPointsResponseDto(true, $"Resgate de {request.Points} pontos realizado com sucesso! Desconto de R$ {valorMonetario:F2} a ser aplicado para o cliente.", valorMonetario);
    }
}
