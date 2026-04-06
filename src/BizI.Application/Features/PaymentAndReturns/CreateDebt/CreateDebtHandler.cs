namespace BizI.Application.Features.PaymentAndReturns.CreateDebt;

public class CreateDebtHandler : IRequestHandler<CreateDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;

    public CreateDebtHandler(IRepository<Debt> repo) => _repo = repo;

    public async Task<CommandResult> Handle(CreateDebtCommand r, CancellationToken ct)
    {
        try
        {
            var debt = Debt.Create(r.CustomerId, r.OrderId, r.Amount, r.Currency);
            await _repo.AddAsync(debt);
            return CommandResult.SuccessResult(debt.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
