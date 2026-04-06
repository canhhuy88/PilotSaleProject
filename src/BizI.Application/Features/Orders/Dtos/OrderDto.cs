using BizI.Domain.Enums;

namespace BizI.Application.Features.Orders.Dtos;

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

public record OrderItemDto(
    Guid ProductId,
    int Quantity,
    int ReturnedQuantity,
    decimal Price,
    decimal LineTotal,
    string Currency);

public record CreateOrderDto(
    Guid CustomerId,
    List<CreateOrderItemDto> Items,
    Guid WarehouseId,
    decimal Discount = 0m,
    string Currency = "VND");

public record CreateOrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal Price);

public record ReturnOrderResultDto(
    Guid OrderId,
    string Status,
    IEnumerable<ReturnItemResultDto> RemainingItems);

public record ReturnItemResultDto(Guid ProductId, int Remaining);

public record ReturnOrderDto(
    Guid OrderId,
    Guid WarehouseId,
    List<ReturnItemDto> Items);

public record ReturnItemDto(Guid ProductId, int Quantity);
