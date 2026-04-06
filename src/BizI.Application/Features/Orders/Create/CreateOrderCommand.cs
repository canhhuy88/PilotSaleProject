using BizI.Application.Features.Orders.Dtos;

namespace BizI.Application.Features.Orders.Create;

public record CreateOrderCommand(
    Guid CustomerId,
    List<CreateOrderItemDto> Items,
    Guid WarehouseId,
    decimal Discount = 0m,
    string Currency = "VND") : IRequest<CommandResult>;
