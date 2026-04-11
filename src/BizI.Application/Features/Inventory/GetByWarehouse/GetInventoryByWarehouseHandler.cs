using AutoMapper;
using BizI.Application.Features.Inventory.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Inventory.GetByWarehouse;

public class GetInventoryByWarehouseHandler : IRequestHandler<GetInventoryByWarehouseQuery, IEnumerable<InventoryDto>>
{
    private readonly IRepository<BizI.Domain.Entities.Inventory> _repo;
    private readonly IMapper _mapper;

    public GetInventoryByWarehouseHandler(IRepository<BizI.Domain.Entities.Inventory> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetInventoryByWarehouseQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.FindAsync(x=>x.WarehouseId== request.WarehouseId);
        return _mapper.Map<IEnumerable<InventoryDto>>(items);
    }
}
