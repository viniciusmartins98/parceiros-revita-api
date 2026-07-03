using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class FuncionarioRepository(DatabaseContext context) : IFuncionarioRepository
{
    public async Task<IReadOnlyCollection<Funcionarios>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Funcionarios
            .Include(f => f.Usuario)
            .AsNoTracking()
            .OrderByDescending(f => f.CriadoEm)
            .ToListAsync(cancellationToken);
    }

    public async Task<Funcionarios?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Funcionarios
            .Include(f => f.Usuario)
            .ThenInclude(u => u.TokensAtualizacao)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);
    }

    public async Task<Funcionarios?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Funcionarios
            .Include(f => f.Usuario)
            .ThenInclude(u => u.TokensAtualizacao)
            .FirstOrDefaultAsync(f => f.Usuario.Id == id, cancellationToken);
    }

    public async Task AddAsync(Funcionarios funcionario, CancellationToken cancellationToken = default)
    {
        await context.Funcionarios.AddAsync(funcionario, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Funcionarios funcionario, CancellationToken cancellationToken = default)
    {
        if (context.Entry(funcionario).State == EntityState.Detached)
        {
            context.Funcionarios.Update(funcionario);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Funcionarios funcionario, CancellationToken cancellationToken = default)
    {
        if (funcionario.Usuario != null)
        {
            if (funcionario.Usuario.TokensAtualizacao != null && funcionario.Usuario.TokensAtualizacao.Any())
            {
                context.TokensAtualizacao.RemoveRange(funcionario.Usuario.TokensAtualizacao);
            }
            context.Usuarios.Remove(funcionario.Usuario);
        }

        context.Funcionarios.Remove(funcionario);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new BusinessRuleException("Não é possível excluir o funcionário, pois ele possui registros vinculados. Considere desativá-lo.");
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Funcionarios.AnyAsync(f => f.Usuario.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Funcionarios.AnyAsync(f => f.Usuario.Email == email && f.Id != id, cancellationToken);
    }
}
