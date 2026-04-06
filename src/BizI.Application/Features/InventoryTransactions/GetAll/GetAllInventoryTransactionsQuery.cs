using BizI.Application.Features.InventoryTransactions.Dtos;

namespace BizI.Application.Features.InventoryTransactions.GetAll;

public record GetAllInventoryTransactionsQuery : IRequest<IEnumerable<InventoryTransactionDto>>;
