using Mediator;

namespace RevitaParceiros.Application.Features.Employees.GetEmployeeById;

public record GetEmployeeByIdRequest(Guid Id) : IRequest<EmployeeDto>;
