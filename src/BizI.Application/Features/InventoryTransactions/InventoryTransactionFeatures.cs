using BizI.Application.DTOs.Inventory;

namespace BizI.Application.Features.InventoryTransactions;

public record GetAllInventoryTransactionsQuery : IRequest<IEnumerable<InventoryTransactionDto>>;

public record GetInventoryTransactionByIdQuery(string Id) : IRequest<InventoryTransactionDto?>;

public class GetAllInventoryTransactionsHandler : IRequestHandler<GetAllInventoryTransactionsQuery, IEnumerable<InventoryTransactionDto>>
{
    private readonly IRepository<InventoryTransaction> _repo;
    private readonly IMapper _mapper;

    public GetAllInventoryTransactionsHandler(IRepository<InventoryTransaction> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryTransactionDto>> Handle(
        GetAllInventoryTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<InventoryTransactionDto>>(transactions);
    }
}

public class GetInventoryTransactionByIdHandler : IRequestHandler<GetInventoryTransactionByIdQuery, InventoryTransactionDto?>
{
    private readonly IRepository<InventoryTransaction> _repo;
    private readonly IMapper _mapper;

    public GetInventoryTransactionByIdHandler(IRepository<InventoryTransaction> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<InventoryTransactionDto?> Handle(
        GetInventoryTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _repo.GetByIdAsync(request.Id);
        return transaction is null ? null : _mapper.Map<InventoryTransactionDto>(transaction);
    }
}
