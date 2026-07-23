using Mediator;

namespace RevitaParceiros.Application.Features.EmployeePurchases.DeleteEmployeePurchase;

public sealed record DeleteEmployeePurchaseCommand(Guid Id) : IRequest;
