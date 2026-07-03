using Mediator;

namespace RevitaParceiros.Application.Features.EmployeePurchases.RegisterEmployeePurchase;

public sealed record RegisterEmployeePurchaseCommand(
    decimal Amount, 
    string? Description, 
    DateTime PurchaseDate, 
    Guid EmployeeId, 
    Guid RegisteredByUserId
) : IRequest<RegisterEmployeePurchaseResponse>;

public sealed record RegisterEmployeePurchaseResponse(Guid Id, DateTime CreatedAt);
