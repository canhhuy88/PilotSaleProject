using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.CreateReturnOrder;

public record CreateReturnOrderCommand(Guid OrderId, Guid WarehouseId, List<CreateReturnItemDto> Items, string Currency = "VND") : IRequest<CommandResult>;
