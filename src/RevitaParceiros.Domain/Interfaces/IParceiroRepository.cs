using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IParceiroRepository
{
    Task<IReadOnlyCollection<Parceiros>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Parceiros?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Parceiros parceiro, CancellationToken cancellationToken = default);
    Task UpdateAsync(Parceiros parceiro, CancellationToken cancellationToken = default);
    Task DeleteAsync(Parceiros parceiro, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default);
}
