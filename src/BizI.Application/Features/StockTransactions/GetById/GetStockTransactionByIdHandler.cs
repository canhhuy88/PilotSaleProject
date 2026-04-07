using AutoMapper;
using BizI.Application.Features.StockTransactions.Dtos;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.StockTransactions.GetById;

public class GetStockTransactionByIdHandler : IRequestHandler<GetStockTransactionByIdQuery, StockTransactionDto?>
{
    private readonly IRepository<StockTransaction> _repository;
    private readonly IMapper _mapper;

    public GetStockTransactionByIdHandler(IRepository<StockTransaction> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StockTransactionDto?> Handle(GetStockTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id);
        return transaction == null ? null : _mapper.Map<StockTransactionDto>(transaction);
    }
}
