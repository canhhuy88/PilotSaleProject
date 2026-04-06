using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Products;

public record DeleteProductCommand(string Id) : IRequest<CommandResult>;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, CommandResult>
{
    private readonly IRepository<Product> _repo;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IRepository<Product> repo, ILogger<DeleteProductHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Product {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
