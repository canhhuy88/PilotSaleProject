using BizI.Application.Features.StockItems.Dtos;

namespace BizI.Application.Features.StockItems.GetAll;

public record GetAllStockItemsQuery : IRequest<IEnumerable<StockItemDto>>;

public class GetAllStockItemsHandler : IRequestHandler<GetAllStockItemsQuery, IEnumerable<StockItemDto>>
{
    public Task<IEnumerable<StockItemDto>> Handle(GetAllStockItemsQuery request, CancellationToken cancellationToken)
        => Task.FromResult<IEnumerable<StockItemDto>>(new List<StockItemDto>());
}
