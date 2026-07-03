using Mediator;
using RevitaParceiros.Domain.Entities;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.EmployeePurchases.RegisterEmployeePurchase;

public sealed class RegisterEmployeePurchaseCommandHandler(
    ICompraFuncionarioRepository compraFuncionarioRepository,
    IFuncionarioRepository funcionarioRepository)
    : IRequestHandler<RegisterEmployeePurchaseCommand, RegisterEmployeePurchaseResponse>
{
    public async ValueTask<RegisterEmployeePurchaseResponse> Handle(RegisterEmployeePurchaseCommand request, CancellationToken cancellationToken)
    {
        var funcionario = await funcionarioRepository.GetByIdAsync(request.EmployeeId, cancellationToken);

        if (funcionario == null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        var novaCompra = new ComprasFuncionarios
        {
            Id = Guid.NewGuid(),
            FuncionarioId = funcionario.Id,
            RegistradoPor = request.RegisteredByUserId,
            Valor = request.Amount,
            Descricao = request.Description,
            DataCompra = request.PurchaseDate,
            CriadoEm = DateTime.UtcNow
        };

        await compraFuncionarioRepository.AddAsync(novaCompra, cancellationToken);

        return new RegisterEmployeePurchaseResponse(novaCompra.Id, novaCompra.CriadoEm);
    }
}
