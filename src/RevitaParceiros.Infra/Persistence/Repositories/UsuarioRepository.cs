using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class UsuarioRepository(DatabaseContext context) : IUsuarioRepository
{
    public Task<Usuarios?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return context.Usuarios
            .Include(u => u.Clientes)
            .Include(u => u.Parceiros)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public Task<Usuarios?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return context.Usuarios
            .Include(u => u.Clientes)
            .Include(u => u.Parceiros)
            .Include(u => u.Funcionarios)
            .Include(u => u.TokensAtualizacao)
            .FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
    {
        return context.Usuarios.AnyAsync(u => u.Email == email, ct);
    }

    public Task<List<Usuarios>> GetAllAsync(CancellationToken ct = default)
    {
        return context.Usuarios
            .Include(u => u.Clientes)
            .Include(u => u.Parceiros)
            .Include(u => u.Funcionarios)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(Usuarios usuario, CancellationToken ct = default)
    {
        await context.Usuarios.AddAsync(usuario, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Usuarios usuario, CancellationToken ct = default)
    {
        if (context.Entry(usuario).State == EntityState.Detached)
        {
            context.Usuarios.Update(usuario);
        }
        await context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(Usuarios usuario, CancellationToken ct = default)
    {
        if (usuario.Clientes != null)
        {
            context.Clientes.Remove(usuario.Clientes);
        }

        if (usuario.Parceiros != null)
        {
            context.Parceiros.Remove(usuario.Parceiros);
        }

        if (usuario.Funcionarios != null)
        {
            context.Funcionarios.Remove(usuario.Funcionarios);
        }

        if (usuario.TokensAtualizacao != null && usuario.TokensAtualizacao.Any())
        {
            context.TokensAtualizacao.RemoveRange(usuario.TokensAtualizacao);
        }

        context.Usuarios.Remove(usuario);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            throw new BusinessRuleException("Não é possível excluir o usuário, pois ele possui registros vinculados (como regras de pontuação, transações, etc). Considere desativá-lo em vez de excluí-lo.");
        }
    }
}
