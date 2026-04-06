using BizI.Application.Features.Warehouses.Dtos;

namespace BizI.Application.Features.Warehouses.GetAll;

public class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesQuery, IEnumerable<WarehouseDto>>
{
    private readonly IRepository<Warehouse> _repo;
    private readonly IMapper _mapper;

    public GetAllWarehousesHandler(IRepository<Warehouse> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<WarehouseDto>> Handle(GetAllWarehousesQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<WarehouseDto>>(await _repo.GetAllAsync());
}
