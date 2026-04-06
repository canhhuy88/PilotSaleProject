namespace BizI.Application.Features.PaymentAndReturns.DeleteDebt;

public record DeleteDebtCommand(Guid Id) : IRequest<CommandResult>;
