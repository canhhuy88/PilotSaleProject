using AutoMapper;
using BizI.Application.Features.Inventory.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Inventory.GetAll;

public class GetAllInventoryHandler : IRequestHandler<GetAllInventoryQuery, IEnumerable<InventoryDto>>
{
    private readonly IInventoryRepository _repo;
    private readonly IMapper _mapper;

    public GetAllInventoryHandler(IInventoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetAllInventoryQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<InventoryDto>>(items);
    }
}
