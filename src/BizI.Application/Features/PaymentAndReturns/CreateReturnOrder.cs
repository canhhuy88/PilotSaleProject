using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.PaymentAndReturns;

public record CreateReturnOrderCommand(string OrderId, decimal TotalRefund, List<ReturnItem> Items) : IRequest<CommandResult>;

public class CreateReturnOrderHandler : IRequestHandler<CreateReturnOrderCommand, CommandResult>
{
    private readonly IRepository<ReturnOrder> _repo;

    public CreateReturnOrderHandler(IRepository<ReturnOrder> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateReturnOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = new ReturnOrder { OrderId = request.OrderId, TotalRefund = request.TotalRefund, Items = request.Items ?? new List<ReturnItem>() };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
