using BizI.Domain.Enums;

namespace BizI.Application.Features.Customers.Dtos;

public record CustomerDto(
    Guid Id,
    string Name,
    string? Phone,
    string? Address,
    CustomerType CustomerType,
    int LoyaltyPoints,
    string LoyaltyTier,
    decimal TotalSpent,
    int TotalOrders,
    decimal DebtTotal,
    decimal DebtLimit,
    DateTime? LastOrderDate);

public record CreateCustomerDto(
    string Name,
    string? Phone = null,
    string? Address = null,
    CustomerType CustomerType = CustomerType.Regular,
    decimal DebtLimit = 0m);

public record UpdateCustomerDto(
    Guid Id,
    string Name,
    string? Phone = null,
    string? Address = null,
    decimal DebtLimit = 0m);
