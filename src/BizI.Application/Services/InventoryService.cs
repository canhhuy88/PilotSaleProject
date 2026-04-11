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
    private readonly IRepository<BizI.Domain.Entities.Inventory> _inventoryRepo;
    private readonly IRepository<InventoryTransaction> _transactionRepo;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IRepository<BizI.Domain.Entities.Inventory> inventoryRepo,
        IRepository<InventoryTransaction> transactionRepo,
        ILogger<InventoryService> logger)
    {
        _inventoryRepo = inventoryRepo;
        _transactionRepo = transactionRepo;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task ImportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null)
    {
        _logger.LogInformation(
            "Importing {Quantity} units. Product: {ProductId}, Warehouse: {WarehouseId}, Ref: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        // Inventory.GetByProductAndWarehouseAsync expects Guid — parse from string
        var productGuid = productId;
        var warehouseGuid = warehouseId;

        var inventory = await _inventoryRepo.FindOneAsync(x=>x.ProductId== productGuid && x.WarehouseId== warehouseGuid);

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
    public async Task ExportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null)
    {
        _logger.LogInformation(
            "Exporting {Quantity} units. Product: {ProductId}, Warehouse: {WarehouseId}, Ref: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var productGuid = productId;
        var warehouseGuid = warehouseId;

        var inventory = await _inventoryRepo.FindOneAsync(x => x.ProductId == productGuid && x.WarehouseId == warehouseGuid);

        if (inventory is null)
            throw new InsufficientStockException(productId, 0, quantity);

        // Domain entity enforces the stock-sufficient rule.
        inventory.DeductStock(quantity);
        await _inventoryRepo.UpdateAsync(inventory);

        await RecordTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Export, referenceId);
    }

    /// <inheritdoc />
    public async Task ReturnStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null)
    {
        _logger.LogInformation(
            "Returning {Quantity} units. Product: {ProductId}, Warehouse: {WarehouseId}, Ref: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var productGuid = productId;
        var warehouseGuid = warehouseId;

        var inventory = await _inventoryRepo.FindOneAsync(x => x.ProductId == productGuid && x.WarehouseId == warehouseGuid);

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
    public async Task AdjustStockAsync(Guid productId, Guid warehouseId, int newQuantity)
    {
        _logger.LogInformation(
            "Adjusting stock to {NewQuantity}. Product: {ProductId}, Warehouse: {WarehouseId}",
            newQuantity, productId, warehouseId);

        var productGuid = productId;
        var warehouseGuid = warehouseId;

        var inventory = await _inventoryRepo.FindOneAsync(x => x.ProductId == productGuid && x.WarehouseId == warehouseGuid);

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
        Guid productId,
        Guid warehouseId,
        int quantity,
        InventoryTransactionType type,
        Guid? referenceId = null)
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
