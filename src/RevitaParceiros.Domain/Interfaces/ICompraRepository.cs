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
        Clientes cliente,
        Parceiros? parceiro,
        CancellationToken cancellationToken = default);

    Task<(decimal TotalAmount, int TotalPoints)> GetPartnerAccumulatedAsync(Guid parceiroId, DateTime? sinceDate = null, CancellationToken cancellationToken = default);

    Task<(decimal TotalAmount, int TotalPoints)> GetClientAccumulatedAsync(Guid clienteId, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Compras>> GetByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Compras>> GetByPartnerIdAsync(Guid partnerId, CancellationToken cancellationToken = default);
}
