using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Warehouses;

public record DeleteWarehouseCommand(string Id) : IRequest<CommandResult>;

public class DeleteWarehouseHandler : IRequestHandler<DeleteWarehouseCommand, CommandResult>
{
    private readonly IRepository<Warehouse> _repo;
    private readonly ILogger<DeleteWarehouseHandler> _logger;

    public DeleteWarehouseHandler(IRepository<Warehouse> repo, ILogger<DeleteWarehouseHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Warehouse {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
