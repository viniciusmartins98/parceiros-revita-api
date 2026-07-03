using System;
using System.Collections.Generic;
using Mediator;

namespace RevitaParceiros.Application.Features.EmployeePurchases.ListEmployeePurchases;

public sealed record ListEmployeePurchasesRequest(Guid FuncionarioId) : IRequest<List<EmployeePurchaseDto>>;
