using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class ClienteRepository(DatabaseContext context) : IClienteRepository
{
    public async Task<IReadOnlyCollection<Usuarios>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Usuarios
            .Include(u => u.Clientes)
            .Where(u => u.Clientes != null)
            .AsNoTracking()
            .OrderByDescending(u => u.CriadoEm)
            .ToListAsync(cancellationToken);
    }

    public async Task<Usuarios?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios
            .Include(u => u.Clientes)
            .Include(u => u.TokensAtualizacao)
            .FirstOrDefaultAsync(u => u.Id == id && u.Clientes != null, cancellationToken);
    }

    public async Task AddAsync(Usuarios usuario, CancellationToken cancellationToken = default)
    {
        await context.Usuarios.AddAsync(usuario, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Usuarios usuario, CancellationToken cancellationToken = default)
    {
        if (context.Entry(usuario).State == EntityState.Detached)
        {
            context.Usuarios.Update(usuario);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Usuarios usuario, CancellationToken cancellationToken = default)
    {
        context.Clientes.Remove(usuario.Clientes);

        if (usuario.TokensAtualizacao != null && usuario.TokensAtualizacao.Any())
        {
            context.TokensAtualizacao.RemoveRange(usuario.TokensAtualizacao);
        }

        context.Usuarios.Remove(usuario);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            throw new BusinessRuleException("Não é possível excluir o cliente, pois ele possui registros vinculados. Considere desativá-lo.");
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Usuarios.AnyAsync(u => u.Email == email && u.Id != id, cancellationToken);
    }
}
