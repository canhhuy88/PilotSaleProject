using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Suppliers;

public record DeleteSupplierCommand(string Id) : IRequest<CommandResult>;

public class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand, CommandResult>
{
    private readonly IRepository<Supplier> _repo;
    private readonly ILogger<DeleteSupplierHandler> _logger;

    public DeleteSupplierHandler(IRepository<Supplier> repo, ILogger<DeleteSupplierHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Supplier {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
