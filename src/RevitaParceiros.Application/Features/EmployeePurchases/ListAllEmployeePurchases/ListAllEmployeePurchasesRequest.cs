using System.Collections.Generic;
using Mediator;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;

public sealed record ListAllEmployeePurchasesRequest(string Period = "week") : IRequest<List<EmployeePurchaseDto>>;
