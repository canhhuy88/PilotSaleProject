using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.PaymentAndReturns;

public record UpdateDebtCommand(string Id, string CustomerId, string OrderId, decimal Amount, decimal PaidAmount, string Status) : IRequest<CommandResult>;

public class UpdateDebtHandler : IRequestHandler<UpdateDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;

    public UpdateDebtHandler(IRepository<Debt> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(UpdateDebtCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.CustomerId = request.CustomerId;
        entity.OrderId = request.OrderId;
        entity.Amount = request.Amount;
        entity.PaidAmount = request.PaidAmount;
        entity.Status = request.Status;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
