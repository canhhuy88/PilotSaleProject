using BizI.Application.Features.StockTransactions.Dtos;

namespace BizI.Application.Features.StockTransactions.GetAll;

public record GetAllStockTransactionsQuery : IRequest<IEnumerable<StockTransactionDto>>;
