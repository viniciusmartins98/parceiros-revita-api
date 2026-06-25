using Mediator;

namespace RevitaParceiros.Application.Features.Employees.DeleteEmployee;

public record DeleteEmployeeRequest(Guid Id) : IRequest;
