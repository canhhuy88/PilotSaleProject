using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.StockItems;

public record DeleteStockItemCommand(string Id) : IRequest<CommandResult>;

public class DeleteStockItemHandler : IRequestHandler<DeleteStockItemCommand, CommandResult>
{
    private readonly IRepository<StockItem> _repo;
    private readonly ILogger<DeleteStockItemHandler> _logger;

    public DeleteStockItemHandler(IRepository<StockItem> repo, ILogger<DeleteStockItemHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteStockItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting StockItem {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
