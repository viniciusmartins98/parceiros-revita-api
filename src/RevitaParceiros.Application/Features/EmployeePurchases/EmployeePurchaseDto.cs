using System;

namespace RevitaParceiros.Application.Features.EmployeePurchases;

public sealed record EmployeePurchaseDto(
    Guid Id, 
    decimal Amount, 
    string? Description,
    DateTime PurchaseDate, 
    string EmployeeName, 
    DateTime CreatedAt
);
