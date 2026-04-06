namespace BizI.Application.Features.Orders.Delete;

public record DeleteOrderCommand(Guid Id) : IRequest<CommandResult>;
