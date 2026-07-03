using System.Collections.Generic;
using Mediator;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;

public sealed record ListAllEmployeePurchasesRequest() : IRequest<List<EmployeePurchaseDto>>;
