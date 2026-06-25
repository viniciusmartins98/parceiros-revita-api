using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IFuncionarioRepository
{
    Task<IReadOnlyCollection<Usuarios>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Usuarios?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Usuarios usuario, CancellationToken cancellationToken = default);
    Task UpdateAsync(Usuarios usuario, CancellationToken cancellationToken = default);
    Task DeleteAsync(Usuarios usuario, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default);
}
