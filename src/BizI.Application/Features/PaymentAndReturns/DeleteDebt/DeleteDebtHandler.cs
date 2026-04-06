namespace BizI.Application.Features.PaymentAndReturns.DeleteDebt;

public class DeleteDebtHandler : IRequestHandler<DeleteDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;

    public DeleteDebtHandler(IRepository<Debt> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteDebtCommand r, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(r.Id);
        if (debt is null) return CommandResult.FailureResult($"Debt '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}
