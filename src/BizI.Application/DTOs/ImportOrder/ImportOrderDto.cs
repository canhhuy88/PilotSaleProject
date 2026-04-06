namespace BizI.Application.DTOs.ImportOrder;

public record ImportOrderDto(
    string Id,
    string SupplierId,
    decimal TotalAmount,
    string Currency,
    string Status,
    DateTime CreatedAt,
    IReadOnlyList<ImportOrderItemDto> Items);

public record ImportOrderItemDto(
    string ProductId,
    int Quantity,
    decimal CostPrice,
    string Currency);

public record CreateImportOrderDto(
    string SupplierId,
    List<CreateImportOrderItemDto> Items);

public record CreateImportOrderItemDto(
    string ProductId,
    int Quantity,
    decimal CostPrice,
    string Currency = "VND");
