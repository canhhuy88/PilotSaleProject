using BizI.Application.Common;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders.Confirm;

public class ConfirmImportOrderHandler : IRequestHandler<ConfirmImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;

    public ConfirmImportOrderHandler(IRepository<ImportOrder> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(ConfirmImportOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repo.GetByIdAsync(request.Id);
        if (order is null)
            return CommandResult.FailureResult($"Import order {request.Id} not found.");

        order.Confirm();
        await _repo.UpdateAsync(order);
        return CommandResult.SuccessResult();
    }
}
