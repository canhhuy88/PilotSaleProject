using BizI.Application.Features.StockOperations.Dtos;

namespace BizI.Application.Features.StockOperations.GetAll;

public record GetAllStockOperationsQuery : IRequest<IEnumerable<StockOperationDto>>;

public class GetAllStockOperationsHandler : IRequestHandler<GetAllStockOperationsQuery, IEnumerable<StockOperationDto>>
{
    public Task<IEnumerable<StockOperationDto>> Handle(GetAllStockOperationsQuery request, CancellationToken cancellationToken)
        => Task.FromResult<IEnumerable<StockOperationDto>>(new List<StockOperationDto>());
}
