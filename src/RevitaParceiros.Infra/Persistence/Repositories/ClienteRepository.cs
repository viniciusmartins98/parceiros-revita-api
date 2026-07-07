using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class ClienteRepository(DatabaseContext context) : IClienteRepository
{
    public async Task<IReadOnlyCollection<Clientes>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Clientes
            .Include(c => c.Usuario)
            .AsNoTracking()
            .OrderByDescending(c => c.CriadoEm)
            .ToListAsync(cancellationToken);
    }

    public async Task<Clientes?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clientes
            .Include(c => c.Usuario)
            .ThenInclude(u => u.TokensAtualizacao)
            .Include(c => c.Compras)
            .Include(c => c.ExtratoPontos)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Clientes?> GetByUserIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clientes
            .Include(c => c.Usuario)
            .ThenInclude(u => u.TokensAtualizacao)
            .Include(c => c.Compras)
            .Include(c => c.ExtratoPontos)
            .FirstOrDefaultAsync(c => c.Usuario.Id == id, cancellationToken);
    }

    public async Task AddAsync(Clientes cliente, CancellationToken cancellationToken = default)
    {
        await context.Clientes.AddAsync(cliente, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Clientes cliente, CancellationToken cancellationToken = default)
    {
        if (context.Entry(cliente).State == EntityState.Detached)
        {
            context.Clientes.Update(cliente);
        }
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Clientes cliente, CancellationToken cancellationToken = default)
    {
        if (cliente.Usuario != null)
        {
            if (cliente.Usuario.TokensAtualizacao != null && cliente.Usuario.TokensAtualizacao.Any())
            {
                context.TokensAtualizacao.RemoveRange(cliente.Usuario.TokensAtualizacao);
            }
            context.Usuarios.Remove(cliente.Usuario);
        }

        context.Clientes.Remove(cliente);

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
        return await context.Clientes.AnyAsync(c => c.Usuario.Email == email, cancellationToken);
    }

    public async Task<bool> ExistsByCpfAsync(string cpf, CancellationToken cancellationToken = default)
    {
        return await context.Clientes.AnyAsync(c => c.Cpf == cpf, cancellationToken);
    }


    public async Task<bool> ExistsByEmailExceptIdAsync(string email, Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Clientes.AnyAsync(c => c.Usuario.Email == email && c.Id != id, cancellationToken);
    }
}
