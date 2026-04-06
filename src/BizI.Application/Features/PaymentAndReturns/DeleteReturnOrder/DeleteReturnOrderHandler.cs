namespace BizI.Application.Features.PaymentAndReturns.DeleteReturnOrder;

public class DeleteReturnOrderHandler : IRequestHandler<DeleteReturnOrderCommand, CommandResult>
{
    private readonly IRepository<ReturnOrder> _repo;

    public DeleteReturnOrderHandler(IRepository<ReturnOrder> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteReturnOrderCommand r, CancellationToken ct)
    {
        var ro = await _repo.GetByIdAsync(r.Id);
        if (ro is null) return CommandResult.FailureResult($"ReturnOrder '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}
