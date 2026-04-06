using AutoMapper;
using BizI.Application.Features.Inventory.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Inventory.GetByProduct;

public class GetInventoryByProductHandler : IRequestHandler<GetInventoryByProductQuery, IEnumerable<InventoryDto>>
{
    private readonly IInventoryRepository _repo;
    private readonly IMapper _mapper;

    public GetInventoryByProductHandler(IInventoryRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetInventoryByProductQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetByProductAsync(request.ProductId);
        return _mapper.Map<IEnumerable<InventoryDto>>(items);
    }
}
