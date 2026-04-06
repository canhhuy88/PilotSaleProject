using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.InventoryTransactions;

public record GetInventoryTransactionByIdQuery(string Id) : IRequest<InventoryTransaction?>;

public class GetInventoryTransactionByIdHandler : IRequestHandler<GetInventoryTransactionByIdQuery, InventoryTransaction?>
{
    private readonly IRepository<InventoryTransaction> _repo;

    public GetInventoryTransactionByIdHandler(IRepository<InventoryTransaction> repo)
    {
        _repo = repo;
    }

    public async Task<InventoryTransaction?> Handle(GetInventoryTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
