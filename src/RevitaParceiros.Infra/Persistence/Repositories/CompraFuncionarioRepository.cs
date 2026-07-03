using Microsoft.EntityFrameworkCore;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Infra.Persistence.Repositories;

public sealed class CompraFuncionarioRepository(DatabaseContext context) : ICompraFuncionarioRepository
{
    public async Task<IReadOnlyCollection<ComprasFuncionarios>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.ComprasFuncionarios
            .Include(c => c.Funcionario)
                .ThenInclude(f => f.Usuario)
            .Include(c => c.RegistradoPorNavigation)
            .AsNoTracking()
            .OrderByDescending(c => c.DataCompra)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<ComprasFuncionarios>> GetByFuncionarioIdAsync(Guid funcionarioId, CancellationToken cancellationToken = default)
    {
        return await context.ComprasFuncionarios
            .Include(c => c.Funcionario)
                .ThenInclude(f => f.Usuario)
            .Include(c => c.RegistradoPorNavigation)
            .Where(c => c.FuncionarioId == funcionarioId)
            .AsNoTracking()
            .OrderByDescending(c => c.DataCompra)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ComprasFuncionarios compra, CancellationToken cancellationToken = default)
    {
        await context.ComprasFuncionarios.AddAsync(compra, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
