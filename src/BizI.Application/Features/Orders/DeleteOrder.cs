using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Orders;

public record DeleteOrderCommand(string Id) : IRequest<CommandResult>;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, CommandResult>
{
    private readonly IRepository<Order> _repo;

    public DeleteOrderHandler(IRepository<Order> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
