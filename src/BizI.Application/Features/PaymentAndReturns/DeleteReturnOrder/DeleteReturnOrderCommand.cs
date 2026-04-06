namespace BizI.Application.Features.PaymentAndReturns.DeleteReturnOrder;

public record DeleteReturnOrderCommand(Guid Id) : IRequest<CommandResult>;
