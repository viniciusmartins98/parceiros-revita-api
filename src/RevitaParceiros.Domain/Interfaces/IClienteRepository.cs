using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IClienteRepository
{
    Task<IReadOnlyCollection<Clientes>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Clientes?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Clientes?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Clientes cliente, CancellationToken cancellationToken = default);
    Task UpdateAsync(Clientes cliente, CancellationToken cancellationToken = default);
    Task DeleteAsync(Clientes cliente, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default);
}
