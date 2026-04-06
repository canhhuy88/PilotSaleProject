namespace BizI.Application.Features.PaymentAndReturns.DeletePayment;

public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _repo;

    public DeletePaymentHandler(IRepository<Payment> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeletePaymentCommand r, CancellationToken ct)
    {
        var payment = await _repo.GetByIdAsync(r.Id);
        if (payment is null) return CommandResult.FailureResult($"Payment '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}
