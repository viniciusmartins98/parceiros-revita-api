using Microsoft.EntityFrameworkCore;
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

    public async Task<(decimal TotalAmount, int TotalPoints)> GetPartnerAccumulatedAsync(Guid parceiroId, CancellationToken cancellationToken = default)
    {
        var result = await context.Compras
            .Where(c => c.ParceiroId == parceiroId)
            .GroupBy(c => c.ParceiroId)
            .Select(g => new
            {
                TotalAmount = g.Sum(c => c.Valor),
                TotalPoints = g.Sum(c => c.PontosGeradosParceiro)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return (result?.TotalAmount ?? 0m, result?.TotalPoints ?? 0);
    }

    public async Task<(decimal TotalAmount, int TotalPoints)> GetClientAccumulatedAsync(Guid clienteId, CancellationToken cancellationToken = default)
    {
        var result = await context.Compras
            .Where(c => c.ClienteId == clienteId)
            .GroupBy(c => c.ClienteId)
            .Select(g => new
            {
                TotalAmount = g.Sum(c => c.Valor),
                TotalPoints = g.Sum(c => c.PontosGeradosCliente)
            })
            .FirstOrDefaultAsync(cancellationToken);

        return (result?.TotalAmount ?? 0m, result?.TotalPoints ?? 0);
    }
}
