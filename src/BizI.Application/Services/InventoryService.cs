using System;
using System.Threading.Tasks;
using System.Linq;
using BizI.Application.Interfaces;
using BizI.Domain.Entities;
using BizI.Domain.Enums;
using BizI.Domain.Interfaces;
using BizI.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Services;

public class InventoryService : IInventoryService
{
    private readonly IRepository<Inventory> _inventoryRepo;
    private readonly IRepository<InventoryTransaction> _transactionRepo;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(
        IRepository<Inventory> inventoryRepo,
        IRepository<InventoryTransaction> transactionRepo,
        ILogger<InventoryService> logger)
    {
        _inventoryRepo = inventoryRepo;
        _transactionRepo = transactionRepo;
        _logger = logger;
    }

    public async Task ImportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null)
    {
        _logger.LogInformation("Importing {Quantity} units of product {ProductId} to warehouse {WarehouseId}. Reference: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var inv = (await _inventoryRepo.FindAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId)).FirstOrDefault();
        if (inv != null)
        {
            _logger.LogInformation("Existing inventory record found for product {ProductId} in warehouse {WarehouseId}. Updating quantity.", productId, warehouseId);
            inv.Quantity += quantity;
            await _inventoryRepo.UpdateAsync(inv);
        }
        else
        {
            _logger.LogInformation("No existing inventory record for product {ProductId} in warehouse {WarehouseId}. Creating new record.", productId, warehouseId);
            await _inventoryRepo.AddAsync(new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = quantity });
        }

        await CreateTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Import, referenceId);
    }

    public async Task ExportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null)
    {
        _logger.LogInformation("Exporting {Quantity} units of product {ProductId} from warehouse {WarehouseId}. Reference: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var inv = (await _inventoryRepo.FindAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId)).FirstOrDefault();
        if (inv == null || inv.Quantity < quantity)
        {
            _logger.LogWarning("Insufficient stock for product {ProductId} in warehouse {WarehouseId}. Current stock: {CurrentQuantity}, Requested: {RequestedQuantity}",
                productId, warehouseId, inv?.Quantity ?? 0, quantity);
            throw new InsufficientStockException(productId);
        }

        inv.Quantity -= quantity;
        await _inventoryRepo.UpdateAsync(inv);

        await CreateTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Export, referenceId);
    }

    public async Task ReturnStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null)
    {
        _logger.LogInformation("Returning {Quantity} units of product {ProductId} to warehouse {WarehouseId}. Reference: {ReferenceId}",
            quantity, productId, warehouseId, referenceId);

        var inv = (await _inventoryRepo.FindAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId)).FirstOrDefault();
        if (inv != null)
        {
            inv.Quantity += quantity;
            await _inventoryRepo.UpdateAsync(inv);
        }
        else
        {
            await _inventoryRepo.AddAsync(new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = quantity });
        }

        await CreateTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Return, referenceId);
    }

    public async Task AdjustStockAsync(Guid productId, Guid warehouseId, int quantity)
    {
        _logger.LogInformation("Adjusting stock for product {ProductId} in warehouse {WarehouseId} to total {Quantity}.",
            productId, warehouseId, quantity);

        var inv = (await _inventoryRepo.FindAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId)).FirstOrDefault();

        if (inv != null)
        {
            int change = quantity - inv.Quantity;
            _logger.LogInformation("Stock adjustment: {OldQuantity} -> {NewQuantity} (Change: {Change})", inv.Quantity, quantity, change);
            inv.Quantity = quantity;
            await _inventoryRepo.UpdateAsync(inv);
            await CreateTransactionAsync(productId, warehouseId, change, InventoryTransactionType.Adjust);
        }
        else
        {
            _logger.LogInformation("Creating new inventory record for stock adjustment.");
            await _inventoryRepo.AddAsync(new Inventory { ProductId = productId, WarehouseId = warehouseId, Quantity = quantity });
            await CreateTransactionAsync(productId, warehouseId, quantity, InventoryTransactionType.Adjust);
        }
    }

    private async Task CreateTransactionAsync(Guid productId, Guid warehouseId, int quantity, InventoryTransactionType type, Guid? referenceId = null)
    {
        _logger.LogInformation("Creating inventory transaction: Type={Type}, Quantity={Quantity}, Product={ProductId}, Warehouse={WarehouseId}, Reference={ReferenceId}",
            type, Math.Abs(quantity), productId, warehouseId, referenceId);

        await _transactionRepo.AddAsync(new InventoryTransaction
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = Math.Abs(quantity),
            Type = type,
            ReferenceId = referenceId
        });
    }
}
