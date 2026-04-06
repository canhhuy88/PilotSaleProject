using AutoMapper;
using BizI.Application.Features.InventoryTransactions.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.InventoryTransactions.GetAll;

public class GetAllInventoryTransactionsHandler : IRequestHandler<GetAllInventoryTransactionsQuery, IEnumerable<InventoryTransactionDto>>
{
    private readonly IRepository<BizI.Domain.Entities.InventoryTransaction> _repo;
    private readonly IMapper _mapper;

    public GetAllInventoryTransactionsHandler(IRepository<BizI.Domain.Entities.InventoryTransaction> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryTransactionDto>> Handle(GetAllInventoryTransactionsQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<InventoryTransactionDto>>(items);
    }
}
