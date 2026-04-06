using BizI.Domain.Enums;

namespace BizI.Application.Features.Inventory.Dtos;

/// <summary>
/// Read DTO for an inventory record.
/// </summary>
public record InventoryDto(
    Guid Id,
    Guid ProductId,
    Guid WarehouseId,
    int Quantity);

/// <summary>
/// Input DTO for importing stock into a warehouse.
/// </summary>
public record ImportStockDto(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null);

/// <summary>
/// Input DTO for exporting stock from a warehouse.
/// </summary>
public record ExportStockDto(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null);

/// <summary>
/// Input DTO for adjusting stock to a new absolute quantity.
/// </summary>
public record AdjustStockDto(
    Guid ProductId,
    Guid WarehouseId,
    int NewQuantity);
