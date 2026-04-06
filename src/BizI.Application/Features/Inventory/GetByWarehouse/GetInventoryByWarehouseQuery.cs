using BizI.Application.Features.Inventory.Dtos;

namespace BizI.Application.Features.Inventory.GetByWarehouse;

public record GetInventoryByWarehouseQuery(Guid WarehouseId) : IRequest<IEnumerable<InventoryDto>>;
