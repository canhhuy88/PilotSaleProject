namespace BizI.Application.Features.PaymentAndReturns.DeletePayment;

public record DeletePaymentCommand(Guid Id) : IRequest<CommandResult>;
