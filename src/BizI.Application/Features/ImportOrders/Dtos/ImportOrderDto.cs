namespace BizI.Application.Features.ImportOrders.Dtos;

public record ImportOrderDto(
    Guid Id,
    Guid SupplierId,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime CreatedAt,
    IReadOnlyList<ImportOrderItemDto> Items);

public record ImportOrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal CostPrice,
    string Currency);

public record CreateImportOrderDto(
    Guid SupplierId,
    List<CreateImportOrderItemDto> Items);

public record CreateImportOrderItemDto(
    Guid ProductId,
    int Quantity,
    decimal CostPrice,
    string Currency = "VND");
