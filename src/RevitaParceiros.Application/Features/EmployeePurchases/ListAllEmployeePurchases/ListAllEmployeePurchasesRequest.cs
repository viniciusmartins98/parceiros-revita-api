using System.Collections.Generic;
using Mediator;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListAllEmployeePurchases;

public sealed record ListAllEmployeePurchasesRequest(string? Period = null, DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<List<EmployeePurchaseDto>>;
