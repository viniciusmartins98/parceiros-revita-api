using Mediator;

namespace RevitaParceiros.Application.Features.Employees.CreateEmployee;

public record CreateEmployeeRequest(
    string Name,
    string Email,
    string Phone,
    bool IsActive) : IRequest<EmployeeDto>;
