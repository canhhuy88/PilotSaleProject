using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.StockTransactions;

public record GetAllStockTransactionsQuery() : IRequest<IEnumerable<StockTransaction>>;

public class GetAllStockTransactionsHandler : IRequestHandler<GetAllStockTransactionsQuery, IEnumerable<StockTransaction>>
{
    private readonly IRepository<StockTransaction> _repo;

    public GetAllStockTransactionsHandler(IRepository<StockTransaction> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockTransaction>> Handle(GetAllStockTransactionsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
