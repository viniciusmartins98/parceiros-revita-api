using System;

namespace RevitaParceiros.Application.Features.Points.Dtos;

public record RecentRedemptionDto(
    Guid Id,
    DateTime Date,
    Guid ProfileId,
    string ProfileName,
    int Points,
    decimal ReaisValue
);
