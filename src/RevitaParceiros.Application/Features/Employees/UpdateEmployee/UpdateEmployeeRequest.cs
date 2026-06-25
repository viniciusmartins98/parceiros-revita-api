using Mediator;

namespace RevitaParceiros.Application.Features.Employees.UpdateEmployee;

public record UpdateEmployeeRequest(
    Guid Id,
    string Name,
    string Email,
    string Phone,
    bool IsActive) : IRequest<EmployeeDto>;
