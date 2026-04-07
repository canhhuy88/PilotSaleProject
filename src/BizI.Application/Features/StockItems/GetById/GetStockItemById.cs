using BizI.Application.Features.StockItems.Dtos;

namespace BizI.Application.Features.StockItems.GetById;

public record GetStockItemByIdQuery(Guid Id) : IRequest<StockItemDto?>;

public class GetStockItemByIdHandler : IRequestHandler<GetStockItemByIdQuery, StockItemDto?>
{
    public Task<StockItemDto?> Handle(GetStockItemByIdQuery request, CancellationToken cancellationToken)
        => Task.FromResult<StockItemDto?>(null);
}
