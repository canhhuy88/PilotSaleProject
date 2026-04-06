namespace BizI.Application.Features.PaymentAndReturns.CreatePayment;

public record CreatePaymentCommand(Guid OrderId, decimal Amount, string Method, string Currency = "VND") : IRequest<CommandResult>;
