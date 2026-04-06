using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.StockItems;

public record GetAllStockItemsQuery() : IRequest<IEnumerable<StockItem>>;

public class GetAllStockItemsHandler : IRequestHandler<GetAllStockItemsQuery, IEnumerable<StockItem>>
{
    private readonly IRepository<StockItem> _repo;

    public GetAllStockItemsHandler(IRepository<StockItem> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockItem>> Handle(GetAllStockItemsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
