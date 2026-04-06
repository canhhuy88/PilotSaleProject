using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.StockTransactions;

public record GetStockTransactionByIdQuery(string Id) : IRequest<StockTransaction?>;

public class GetStockTransactionByIdHandler : IRequestHandler<GetStockTransactionByIdQuery, StockTransaction?>
{
    private readonly IRepository<StockTransaction> _repo;

    public GetStockTransactionByIdHandler(IRepository<StockTransaction> repo)
    {
        _repo = repo;
    }

    public async Task<StockTransaction?> Handle(GetStockTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
