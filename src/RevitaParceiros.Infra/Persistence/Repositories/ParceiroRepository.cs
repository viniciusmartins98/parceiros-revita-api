using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class ParceiroRepository(DatabaseContext context) : IParceiroRepository
{
    public async Task<IReadOnlyCollection<Parceiros>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Parceiros
            .Include(p => p.Usuario)
            .AsNoTracking()
            .OrderByDescending(p => p.CriadoEm)
            .ToListAsync(cancellationToken);
    }

    public async Task<Parceiros?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Parceiros
            .Include(p => p.Usuario)
            .ThenInclude(u => u.TokensAtualizacao)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task AddAsync(Parceiros parceiro, CancellationToken cancellationToken = default)
    {
        await context.Parceiros.AddAsync(parceiro, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Parceiros parceiro, CancellationToken cancellationToken = default)
    {
        if (context.Entry(parceiro).State == EntityState.Detached)
        {
            context.Parceiros.Update(parceiro);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Parceiros parceiro, CancellationToken cancellationToken = default)
    {
        if (parceiro.Usuario != null)
        {
            if (parceiro.Usuario.TokensAtualizacao != null && parceiro.Usuario.TokensAtualizacao.Any())
            {
                context.TokensAtualizacao.RemoveRange(parceiro.Usuario.TokensAtualizacao);
            }
            context.Usuarios.Remove(parceiro.Usuario);
        }

        context.Parceiros.Remove(parceiro);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new BusinessRuleException("Não é possível excluir o parceiro, pois ele possui registros vinculados. Considere desativá-lo.");
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Parceiros.AnyAsync(p => p.Usuario.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Parceiros.AnyAsync(p => p.Usuario.Email == email && p.Id != id, cancellationToken);
    }
}
