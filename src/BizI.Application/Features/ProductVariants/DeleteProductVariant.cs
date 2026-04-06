using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.ProductVariants;

public record DeleteProductVariantCommand(string Id) : IRequest<CommandResult>;

public class DeleteProductVariantHandler : IRequestHandler<DeleteProductVariantCommand, CommandResult>
{
    private readonly IRepository<ProductVariant> _repo;
    private readonly ILogger<DeleteProductVariantHandler> _logger;

    public DeleteProductVariantHandler(IRepository<ProductVariant> repo, ILogger<DeleteProductVariantHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteProductVariantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting ProductVariant {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
