using BizI.Application.Common;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders.Receive;

public class ReceiveImportOrderHandler : IRequestHandler<ReceiveImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;

    public ReceiveImportOrderHandler(IRepository<ImportOrder> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(ReceiveImportOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repo.GetByIdAsync(request.Id);
        if (order is null)
            return CommandResult.FailureResult($"Import order {request.Id} not found.");

        order.MarkReceived();
        await _repo.UpdateAsync(order);
        return CommandResult.SuccessResult();
    }
}
