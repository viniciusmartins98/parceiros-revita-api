using Mediator;

namespace RevitaParceiros.Application.Features.Employees.ListEmployees;

public record ListEmployeesRequest : IRequest<IReadOnlyCollection<EmployeeDto>>;
