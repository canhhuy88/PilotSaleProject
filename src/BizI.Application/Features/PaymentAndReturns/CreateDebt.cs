using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.PaymentAndReturns;

public record CreateDebtCommand(string CustomerId, string OrderId, decimal Amount, decimal PaidAmount, string Status) : IRequest<CommandResult>;

public class CreateDebtHandler : IRequestHandler<CreateDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;

    public CreateDebtHandler(IRepository<Debt> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateDebtCommand request, CancellationToken cancellationToken)
    {
        var entity = new Debt { CustomerId = request.CustomerId, OrderId = request.OrderId, Amount = request.Amount, PaidAmount = request.PaidAmount, Status = request.Status };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
