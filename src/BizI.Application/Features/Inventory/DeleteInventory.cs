using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Inventory;

public record DeleteInventoryCommand(string Id) : IRequest<CommandResult>;

public class DeleteInventoryHandler : IRequestHandler<DeleteInventoryCommand, CommandResult>
{
    private readonly IRepository<BizI.Domain.Entities.Inventory> _repo;

    public DeleteInventoryHandler(IRepository<BizI.Domain.Entities.Inventory> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(DeleteInventoryCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
