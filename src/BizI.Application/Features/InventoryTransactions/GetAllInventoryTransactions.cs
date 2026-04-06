using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.InventoryTransactions;

public record GetAllInventoryTransactionsQuery() : IRequest<IEnumerable<InventoryTransaction>>;

public class GetAllInventoryTransactionsHandler : IRequestHandler<GetAllInventoryTransactionsQuery, IEnumerable<InventoryTransaction>>
{
    private readonly IRepository<InventoryTransaction> _repo;

    public GetAllInventoryTransactionsHandler(IRepository<InventoryTransaction> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<InventoryTransaction>> Handle(GetAllInventoryTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
