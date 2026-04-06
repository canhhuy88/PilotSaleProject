using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.StockItems;

public record GetStockItemByIdQuery(string Id) : IRequest<StockItem?>;

public class GetStockItemByIdHandler : IRequestHandler<GetStockItemByIdQuery, StockItem?>
{
    private readonly IRepository<StockItem> _repo;

    public GetStockItemByIdHandler(IRepository<StockItem> repo)
    {
        _repo = repo;
    }

    public async Task<StockItem?> Handle(GetStockItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
