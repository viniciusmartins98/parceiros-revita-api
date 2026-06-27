using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

/// <summary>
/// Implementação do repositório de compras.
/// Persiste a compra, o extrato de pontos e a atualização do parceiro em uma única transação.
/// </summary>
public sealed class CompraRepository(DatabaseContext context) : ICompraRepository
{
    public async Task RegisterSaleAsync(
        Compras compra,
        IEnumerable<ExtratoPontos> extratoPontos,
        Parceiros parceiro,
        Clientes cliente,
        CancellationToken cancellationToken = default)
    {
        await context.Compras.AddAsync(compra, cancellationToken);
        await context.ExtratoPontos.AddRangeAsync(extratoPontos, cancellationToken);
        context.Parceiros.Update(parceiro);
        context.Clientes.Update(cliente);

        await context.SaveChangesAsync(cancellationToken);
    }
}
