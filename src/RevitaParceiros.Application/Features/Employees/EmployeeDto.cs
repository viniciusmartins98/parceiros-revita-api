namespace RevitaParceiros.Application.Features.Employees;

public record EmployeeDto(
    Guid Id,
    string Name,
    string Phone,
    string Email,
    bool IsActive,
    DateTime CreatedAt);
