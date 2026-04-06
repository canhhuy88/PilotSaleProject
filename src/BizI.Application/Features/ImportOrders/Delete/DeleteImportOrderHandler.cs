using BizI.Application.Common;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders.Delete;

public class DeleteImportOrderHandler : IRequestHandler<DeleteImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;

    public DeleteImportOrderHandler(IRepository<ImportOrder> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(DeleteImportOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _repo.GetByIdAsync(request.Id);
        if (order is null)
            return CommandResult.FailureResult($"Import order {request.Id} not found.");

        order.Cancel();
        await _repo.UpdateAsync(order);
        return CommandResult.SuccessResult();
    }
}
