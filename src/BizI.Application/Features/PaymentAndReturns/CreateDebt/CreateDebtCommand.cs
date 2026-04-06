namespace BizI.Application.Features.PaymentAndReturns.CreateDebt;

public record CreateDebtCommand(Guid CustomerId, Guid OrderId, decimal Amount, string Currency = "VND") : IRequest<CommandResult>;
