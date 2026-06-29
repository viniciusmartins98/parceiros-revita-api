using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IResgatesRepository
{
    Task AddAsync(Resgates resgate, CancellationToken cancellationToken = default);
    Task<IEnumerable<Resgates>> GetRecentAsync(int limit, CancellationToken cancellationToken = default);
}
