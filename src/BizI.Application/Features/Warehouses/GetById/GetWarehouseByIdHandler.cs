using BizI.Application.Features.Warehouses.Dtos;

namespace BizI.Application.Features.Warehouses.GetById;

public class GetWarehouseByIdHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDto?>
{
    private readonly IRepository<Warehouse> _repo;
    private readonly IMapper _mapper;

    public GetWarehouseByIdHandler(IRepository<Warehouse> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<WarehouseDto?> Handle(GetWarehouseByIdQuery r, CancellationToken ct)
    {
        var w = await _repo.GetByIdAsync(r.Id);
        return w is null ? null : _mapper.Map<WarehouseDto>(w);
    }
}
