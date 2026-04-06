using BizI.Application.Features.InventoryTransactions.Dtos;

namespace BizI.Application.Features.InventoryTransactions.GetById;

public record GetInventoryTransactionByIdQuery(Guid Id) : IRequest<InventoryTransactionDto?>;
