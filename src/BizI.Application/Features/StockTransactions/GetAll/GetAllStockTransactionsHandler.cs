using AutoMapper;
using BizI.Application.Features.StockTransactions.Dtos;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.StockTransactions.GetAll;

public class GetAllStockTransactionsHandler : IRequestHandler<GetAllStockTransactionsQuery, IEnumerable<StockTransactionDto>>
{
    private readonly IRepository<StockTransaction> _repository;
    private readonly IMapper _mapper;

    public GetAllStockTransactionsHandler(IRepository<StockTransaction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StockTransactionDto>> Handle(GetAllStockTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StockTransactionDto>>(transactions);
    }
}
