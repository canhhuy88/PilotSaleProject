using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.PaymentAndReturns;

public record DeleteReturnOrderCommand(string Id) : IRequest<CommandResult>;

public class DeleteReturnOrderHandler : IRequestHandler<DeleteReturnOrderCommand, CommandResult>
{
    private readonly IRepository<ReturnOrder> _repo;

    public DeleteReturnOrderHandler(IRepository<ReturnOrder> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(DeleteReturnOrderCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
