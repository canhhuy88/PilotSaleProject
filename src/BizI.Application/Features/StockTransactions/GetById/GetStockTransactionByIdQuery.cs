using BizI.Application.Features.StockTransactions.Dtos;

namespace BizI.Application.Features.StockTransactions.GetById;

public record GetStockTransactionByIdQuery(Guid Id) : IRequest<StockTransactionDto?>;
