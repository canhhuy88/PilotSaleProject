using BizI.Application.Interfaces;
using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Exceptions;
using BizI.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Services;

/// <summary>
/// Application service responsible for inventory stock management.
/// Uses domain repository interfaces only — no direct database access.
/// All IDs are strings to align with the refactored Domain.
/// </summary>
public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IRepository<InventoryTransaction> _transactionRepo;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IInventoryRepository inventoryRepo,
        IRepository<InventoryTransaction> transactionRepo,
        ILogger<InventoryService> logger)
    {
        _inventoryRepo = inventoryRepo;
        _transactionRepo = transactionRepo;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ImportStockAsync(string productId, string warehouseId, int quantity, string? referenceId = null)
    {
        _logger.LogInformation(
            "Importing {Quantity} units. Product: {ProductId}, Warehouse: {WarehouseId}, Ref: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        // Inventory.GetByProductAndWarehouseAsync expects Guid — parse from string
        var productGuid = Guid.Parse(productId);
        var warehouseGuid = Guid.Parse(warehouseId);

        var inventory = await _inventoryRepo.GetByProductAndWarehouseAsync(productGuid, warehouseGuid);

        if (inventory is not null)
        {
            inventory.AddStock(quantity);
            await _inventoryRepo.UpdateAsync(inventory);
        }
        else
        {
            inventory = Inventory.Create(productId, warehouseId, quantity);
            await _inventoryRepo.AddAsync(inventory);
        }

        await RecordTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Import, referenceId);
    }

    /// <inheritdoc />
    public async Task ExportStockAsync(string productId, string warehouseId, int quantity, string? referenceId = null)
    {
        _logger.LogInformation(
            "Exporting {Quantity} units. Product: {ProductId}, Warehouse: {WarehouseId}, Ref: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var productGuid = Guid.Parse(productId);
        var warehouseGuid = Guid.Parse(warehouseId);

        var inventory = await _inventoryRepo.GetByProductAndWarehouseAsync(productGuid, warehouseGuid);

        if (inventory is null)
            throw new InsufficientStockException(productId, 0, quantity);

        // Domain entity enforces the stock-sufficient rule.
        inventory.DeductStock(quantity);
        await _inventoryRepo.UpdateAsync(inventory);

        await RecordTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Export, referenceId);
    }

    /// <inheritdoc />
    public async Task ReturnStockAsync(string productId, string warehouseId, int quantity, string? referenceId = null)
    {
        _logger.LogInformation(
            "Returning {Quantity} units. Product: {ProductId}, Warehouse: {WarehouseId}, Ref: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var productGuid = Guid.Parse(productId);
        var warehouseGuid = Guid.Parse(warehouseId);

        var inventory = await _inventoryRepo.GetByProductAndWarehouseAsync(productGuid, warehouseGuid);

        if (inventory is not null)
        {
            inventory.AddStock(quantity);
            await _inventoryRepo.UpdateAsync(inventory);
        }
        else
        {
            inventory = Inventory.Create(productId, warehouseId, quantity);
            await _inventoryRepo.AddAsync(inventory);
        }

        await RecordTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Return, referenceId);
    }

    /// <inheritdoc />
    public async Task AdjustStockAsync(string productId, string warehouseId, int newQuantity)
    {
        _logger.LogInformation(
            "Adjusting stock to {NewQuantity}. Product: {ProductId}, Warehouse: {WarehouseId}",
            newQuantity, productId, warehouseId);

        var productGuid = Guid.Parse(productId);
        var warehouseGuid = Guid.Parse(warehouseId);

        var inventory = await _inventoryRepo.GetByProductAndWarehouseAsync(productGuid, warehouseGuid);

        int delta;
        if (inventory is not null)
        {
            delta = inventory.SetQuantity(newQuantity);
            await _inventoryRepo.UpdateAsync(inventory);
        }
        else
        {
            inventory = Inventory.Create(productId, warehouseId, newQuantity);
            await _inventoryRepo.AddAsync(inventory);
            delta = newQuantity;
        }

        _logger.LogInformation("Stock adjusted. Delta: {Delta}", delta);
        await RecordTransactionAsync(productId, warehouseId, delta, InventoryTransactionType.Adjust);
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private async Task RecordTransactionAsync(
        string productId,
        string warehouseId,
        int quantity,
        InventoryTransactionType type,
        string? referenceId = null)
    {
        // Use Domain factory — respects encapsulation (private setters)
        var transaction = InventoryTransaction.Create(
            productId,
            warehouseId,
            type,
            Math.Abs(quantity),
            referenceId);

        await _transactionRepo.AddAsync(transaction);

        _logger.LogDebug(
            "Inventory transaction recorded. Type: {Type}, Qty: {Qty}, Product: {ProductId}",
            type, Math.Abs(quantity), productId);
    }
}
