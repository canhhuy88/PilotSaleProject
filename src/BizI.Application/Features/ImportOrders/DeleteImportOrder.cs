using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.ImportOrders;

public record DeleteImportOrderCommand(string Id) : IRequest<CommandResult>;

public class DeleteImportOrderHandler : IRequestHandler<DeleteImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly ILogger<DeleteImportOrderHandler> _logger;

    public DeleteImportOrderHandler(IRepository<ImportOrder> repo, ILogger<DeleteImportOrderHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteImportOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting ImportOrder {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
