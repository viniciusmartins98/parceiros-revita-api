using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

/// <summary>
/// Repositório para operações sobre a entidade Compras.
/// </summary>
public interface ICompraRepository
{
    /// <summary>
    /// Registra uma compra, cria o lançamento no extrato de pontos e atualiza o saldo do parceiro,
    /// tudo dentro de uma única transação.
    /// </summary>
    Task RegisterSaleAsync(
        Compras compra,
        IEnumerable<ExtratoPontos> extratoPontos,
        Parceiros parceiro,
        Clientes cliente,
        CancellationToken cancellationToken = default);

    Task<(decimal TotalAmount, int TotalPoints)> GetPartnerAccumulatedAsync(Guid parceiroId, CancellationToken cancellationToken = default);

    Task<(decimal TotalAmount, int TotalPoints)> GetClientAccumulatedAsync(Guid clienteId, CancellationToken cancellationToken = default);
}
