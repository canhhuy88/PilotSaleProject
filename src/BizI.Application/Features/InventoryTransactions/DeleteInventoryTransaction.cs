using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.InventoryTransactions;

public record DeleteInventoryTransactionCommand(string Id) : IRequest<CommandResult>;

public class DeleteInventoryTransactionHandler : IRequestHandler<DeleteInventoryTransactionCommand, CommandResult>
{
    private readonly IRepository<InventoryTransaction> _repo;
    private readonly ILogger<DeleteInventoryTransactionHandler> _logger;

    public DeleteInventoryTransactionHandler(IRepository<InventoryTransaction> repo, ILogger<DeleteInventoryTransactionHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting InventoryTransaction {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
