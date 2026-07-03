using RevitaParceiros.Domain.Entities;

namespace RevitaParceiros.Domain.Interfaces;

public interface IFuncionarioRepository
{
    Task<IReadOnlyCollection<Funcionarios>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Funcionarios?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Funcionarios?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Funcionarios funcionario, CancellationToken cancellationToken = default);
    Task UpdateAsync(Funcionarios funcionario, CancellationToken cancellationToken = default);
    Task DeleteAsync(Funcionarios funcionario, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default);
}
