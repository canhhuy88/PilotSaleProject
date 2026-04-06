using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using BizI.Domain.Entities;

namespace BizI.Application.Interfaces;

public interface IInventoryService
{
    Task ImportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null);
    Task ExportStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null);
    Task ReturnStockAsync(Guid productId, Guid warehouseId, int quantity, Guid? referenceId = null);
    Task AdjustStockAsync(Guid productId, Guid warehouseId, int quantity);
}
