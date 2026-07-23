using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class CompraFuncionarioRepository(DatabaseContext context) : ICompraFuncionarioRepository
{
    public async Task<IReadOnlyCollection<ComprasFuncionarios>> GetAllAsync(DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.ComprasFuncionarios
            .Include(c => c.Funcionario)
                .ThenInclude(f => f.Usuario)
            .Include(c => c.RegistradoPorNavigation)
            .AsNoTracking();
            
        if (startDate.HasValue)
        {
            query = query.Where(c => c.DataCompra >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(c => c.DataCompra <= endDate.Value);
        }

        return await query
            .OrderByDescending(c => c.DataCompra)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ComprasFuncionarios>> GetByFuncionarioIdAsync(Guid funcionarioId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var query = context.ComprasFuncionarios
            .Include(c => c.Funcionario)
                .ThenInclude(f => f.Usuario)
            .Include(c => c.RegistradoPorNavigation)
            .Where(c => c.FuncionarioId == funcionarioId)
            .AsNoTracking();
            
        if (startDate.HasValue)
        {
            query = query.Where(c => c.DataCompra >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(c => c.DataCompra <= endDate.Value);
        }

        return await query
            .OrderByDescending(c => c.DataCompra)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ComprasFuncionarios compra, CancellationToken cancellationToken = default)
    {
        await context.ComprasFuncionarios.AddAsync(compra, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var compra = await context.ComprasFuncionarios.FindAsync([id], cancellationToken);
        if (compra != null)
        {
            context.ComprasFuncionarios.Remove(compra);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
