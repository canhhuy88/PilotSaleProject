using BizI.Domain.Enums;

namespace BizI.Application.DTOs.Order;

/// <summary>
/// Read DTO for a sales order — returned to API, not the Domain entity.
/// </summary>
public record OrderDto(
    Guid Id,
    string Code,
    Guid CustomerId,
    decimal TotalAmount,
    decimal Discount,
    decimal FinalAmount,
    string Currency,
    OrderStatus Status,
    PaymentStatus PaymentStatus,
    string CreatedBy,
    DateTime CreatedAt,
    IReadOnlyList<OrderItemDto> Items);

/// <summary>
/// Read DTO for a single order line item.
/// </summary>
public record OrderItemDto(
    Guid ProductId,
    int Quantity,
    int ReturnedQuantity,
    decimal Price,
    decimal LineTotal,
    string Currency);

/// <summary>
/// Input DTO for creating a new order.
/// </summary>
public record CreateOrderDto(
    Guid CustomerId,
    List<CreateOrderItemDto> Items,
    Guid WarehouseId,
    decimal Discount = 0m,
    string Currency = "VND");

/// <summary>
/// Input line-item DTO inside CreateOrderDto.
/// </summary>
public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal Price);

/// <summary>
/// Read DTO returned after a successful return operation.
/// </summary>
public record ReturnOrderResultDto(
    Guid OrderId,
    string Status,
    IEnumerable<ReturnItemResultDto> RemainingItems);

public record ReturnItemResultDto(Guid ProductId, int Remaining);

/// <summary>
/// Input DTO for returning items from an order.
/// </summary>
public record ReturnOrderDto(
    Guid OrderId,
    Guid WarehouseId,
    List<ReturnItemDto> Items);

public record ReturnItemDto(Guid ProductId, int Quantity);
