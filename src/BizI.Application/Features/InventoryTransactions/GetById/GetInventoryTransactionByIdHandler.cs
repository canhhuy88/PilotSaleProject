using AutoMapper;
using BizI.Application.Features.InventoryTransactions.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.InventoryTransactions.GetById;

public class GetInventoryTransactionByIdHandler : IRequestHandler<GetInventoryTransactionByIdQuery, InventoryTransactionDto?>
{
    private readonly IRepository<BizI.Domain.Entities.InventoryTransaction> _repo;
    private readonly IMapper _mapper;

    public GetInventoryTransactionByIdHandler(IRepository<BizI.Domain.Entities.InventoryTransaction> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<InventoryTransactionDto?> Handle(GetInventoryTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item is null ? null : _mapper.Map<InventoryTransactionDto>(item);
    }
}
