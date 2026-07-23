namespace RevitaParceiros.Application.Features.Partners.GetPartnerProgress;

public sealed record PartnerProgressDto(
    Guid ParceiroId,
    int SaldoPontos,
    decimal FaturamentoVinculado,
    IEnumerable<FaixasPontuacaoDto> FaixasPontuacao
);
