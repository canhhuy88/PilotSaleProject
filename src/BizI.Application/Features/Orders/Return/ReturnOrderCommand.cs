using BizI.Application.Features.Orders.Dtos;

namespace BizI.Application.Features.Orders.Return;

public record ReturnOrderCommand(
    Guid OrderId,
    Guid WarehouseId,
    List<ReturnItemDto> Items) : IRequest<CommandResult>;
