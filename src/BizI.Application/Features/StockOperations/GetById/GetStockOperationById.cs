using BizI.Application.Features.StockOperations.Dtos;

namespace BizI.Application.Features.StockOperations.GetById;

public record GetStockOperationByIdQuery(Guid Id) : IRequest<StockOperationDto?>;

public class GetStockOperationByIdHandler : IRequestHandler<GetStockOperationByIdQuery, StockOperationDto?>
{
    public Task<StockOperationDto?> Handle(GetStockOperationByIdQuery request, CancellationToken cancellationToken)
        => Task.FromResult<StockOperationDto?>(null);
}
