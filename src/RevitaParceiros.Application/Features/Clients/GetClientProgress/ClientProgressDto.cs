namespace RevitaParceiros.Application.Features.Clients.GetClientProgress;

public sealed record ClientProgressDto(
    Guid ClienteId,
    int SaldoPontos,
    decimal FaturamentoResidual,
    decimal ValorParaProximoPonto,
    decimal PontosParaProximaMeta
);
