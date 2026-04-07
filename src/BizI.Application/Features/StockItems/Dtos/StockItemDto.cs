namespace BizI.Application.Features.StockItems.Dtos;

public record StockItemDto(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    int ReservedQty,
    int AvailableQty
);
