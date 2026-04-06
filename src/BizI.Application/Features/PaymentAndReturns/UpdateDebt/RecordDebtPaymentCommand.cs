namespace BizI.Application.Features.PaymentAndReturns.UpdateDebt;

public record RecordDebtPaymentCommand(Guid DebtId, decimal PaidAmount, string Currency = "VND") : IRequest<CommandResult>;
