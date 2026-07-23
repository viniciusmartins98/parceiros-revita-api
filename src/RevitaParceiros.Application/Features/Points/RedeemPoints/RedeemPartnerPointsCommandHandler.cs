using Mediator;
using RevitaParceiros.Application.Features.Points.Dtos;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Enums;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Points.RedeemPoints;

public class RedeemPartnerPointsCommandHandler(
    IParceiroRepository parceiroRepository,
    IExtratoPontosRepository extratoPontosRepository,
    IResgatesRepository resgatesRepository,
    IRegrasPontuacaoRepository regrasPontuacaoRepository) : IRequestHandler<RedeemPartnerPointsCommand, RedeemPointsResponseDto>
{
    public async ValueTask<RedeemPointsResponseDto> Handle(RedeemPartnerPointsCommand request, CancellationToken cancellationToken)
    {
        var config = await regrasPontuacaoRepository.GetActiveConfigAsync(cancellationToken);
        if (config == null)
            throw new BusinessRuleException("Nenhuma regra de pontuação ativa encontrada.");

        var parceiro = await parceiroRepository.GetByIdAsync(request.PartnerId, cancellationToken);
        if (parceiro == null)
            throw new BusinessRuleException("Parceiro não encontrado.");

        if (request.Points == 0)
            throw new BusinessRuleException("Valor de resgate precisa ser maior que zero.");

        if (parceiro.TotalPontos <= 0)
            throw new BusinessRuleException("Parceiro não tem pontos para serem resgatados.");

        if (request.Points != parceiro.TotalPontos)
            throw new BusinessRuleException("É permitido apenas o resgate total dos pontos do parceiro.");

        decimal valorMonetario = (request.Points / config.PontosParaConversaoMonetariaParceiro) * config.ValorMonetarioPorPontosParceiro;

        parceiro.TotalPontos -= request.Points;

        var extrato = new ExtratoPontos
        {
            ParceiroId = parceiro.Id,
            Pontos = -request.Points,
            ValorMonetario = valorMonetario,
            Descricao = $"Resgate de {request.Points} pontos concedendo um desconto de R$ {valorMonetario:F2} para o parceiro.",
            TipoTransacao = TipoTransacaoPontosEnum.Resgate,
            CriadoEm = DateTime.UtcNow
        };

        var resgate = new Resgates
        {
            ParceiroId = parceiro.Id,
            PontosResgatados = request.Points,
            ValorMonetario = valorMonetario,
            AprovadoPor = request.LoggedUserId,
            Status = StatusResgateEnum.Aprovado,
            Observacoes = "Resgate processado automaticamente para o parceiro",
            CriadoEm = DateTime.UtcNow
        };

        await extratoPontosRepository.AddAsync(extrato, cancellationToken);
        await resgatesRepository.AddAsync(resgate, cancellationToken);
        await parceiroRepository.UpdateAsync(parceiro, cancellationToken);

        return new RedeemPointsResponseDto(true, $"Resgate de {request.Points} pontos realizado com sucesso! Desconto de R$ {valorMonetario:F2} a ser aplicado para o parceiro.", valorMonetario);
    }
}
