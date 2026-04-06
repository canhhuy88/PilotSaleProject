namespace BizI.Application.Features.PaymentAndReturns.UpdateDebt;

public class RecordDebtPaymentHandler : IRequestHandler<RecordDebtPaymentCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;

    public RecordDebtPaymentHandler(IRepository<Debt> repo) => _repo = repo;

    public async Task<CommandResult> Handle(RecordDebtPaymentCommand r, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(r.DebtId);
        if (debt is null) return CommandResult.FailureResult($"Debt '{r.DebtId}' not found.");
        try
        {
            debt.RecordPayment(r.PaidAmount, r.Currency);  // ✅ Domain method
            await _repo.UpdateAsync(debt);
            return CommandResult.SuccessResult(debt.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
