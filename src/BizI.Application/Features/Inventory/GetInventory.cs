namespace BizI.Application.Features.Inventory;

public record GetInventoryQuery(Guid? ProductId, Guid? WarehouseId) : IRequest<IEnumerable<BizI.Domain.Entities.Inventory>>;

public class GetInventoryHandler : IRequestHandler<GetInventoryQuery, IEnumerable<BizI.Domain.Entities.Inventory>>
{
    private readonly IRepository<BizI.Domain.Entities.Inventory> _inventoryRepo;

    public GetInventoryHandler(IRepository<BizI.Domain.Entities.Inventory> inventoryRepo)
    {
        _inventoryRepo = inventoryRepo;
    }

    public async Task<IEnumerable<BizI.Domain.Entities.Inventory>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        if (request.ProductId.HasValue && request.WarehouseId.HasValue)
            return await _inventoryRepo.FindAsync(x => x.ProductId == request.ProductId && x.WarehouseId == request.WarehouseId);
        if (request.ProductId.HasValue)
            return await _inventoryRepo.FindAsync(x => x.ProductId == request.ProductId);
        if (request.WarehouseId.HasValue)
            return await _inventoryRepo.FindAsync(x => x.WarehouseId == request.WarehouseId);

        return await _inventoryRepo.GetAllAsync();
    }
}
