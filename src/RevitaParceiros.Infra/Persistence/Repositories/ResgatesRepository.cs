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
}
