using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class ResgatesRepository(DatabaseContext context) : IResgatesRepository
{
    public async Task AddAsync(Resgates resgate, CancellationToken cancellationToken = default)
    {
        await context.Resgates.AddAsync(resgate, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Resgates>> GetRecentAsync(int limit, CancellationToken cancellationToken = default)
    {
        return await context.Resgates
            .Include(r => r.Cliente)
                .ThenInclude(c => c.Usuario)
            .Include(r => r.Parceiro)
                .ThenInclude(p => p.Usuario)
            .OrderByDescending(r => r.CriadoEm)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}
