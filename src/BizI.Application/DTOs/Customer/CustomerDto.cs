using BizI.Domain.Enums;

namespace BizI.Application.DTOs.Customer;

/// <summary>
/// Read DTO for a customer — returned to API, not the Domain entity.
/// </summary>
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

/// <summary>
/// Input DTO for creating a customer.
/// </summary>
public record CreateCustomerDto(
    string Name,
    string? Phone = null,
    string? Address = null,
    CustomerType CustomerType = CustomerType.Regular,
    decimal DebtLimit = 0m);

/// <summary>
/// Input DTO for updating a customer.
/// </summary>
public record UpdateCustomerDto(
    Guid Id,
    string Name,
    string? Phone = null,
    string? Address = null,
    decimal DebtLimit = 0m);
