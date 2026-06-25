using Mediator;
using RevitaParceiros.Domain.Exceptions;
using RevitaParceiros.Domain.Interfaces;

namespace RevitaParceiros.Application.Features.Employees.GetEmployeeById;

public sealed class GetEmployeeByIdHandler(IFuncionarioRepository funcionarioRepository)
    : IRequestHandler<GetEmployeeByIdRequest, EmployeeDto>
{
    public async ValueTask<EmployeeDto> Handle(GetEmployeeByIdRequest request, CancellationToken cancellationToken)
    {
        var usuario = await funcionarioRepository.GetByIdAsync(request.Id, cancellationToken);

        if (usuario is null)
        {
            throw new NotFoundException("Funcionário não encontrado.");
        }

        return new EmployeeDto(
            usuario.Id,
            usuario.Nome,
            usuario.Telefone,
            usuario.Email,
            usuario.Ativo,
            usuario.CriadoEm);
    }
}
