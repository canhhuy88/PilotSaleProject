using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products.Delete;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, CommandResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(IProductRepository productRepository, ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting product. Id: {Id}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product is null)
            return CommandResult.FailureResult($"Product '{request.Id}' not found.");

        await _productRepository.DeleteAsync(request.Id);
        _logger.LogInformation("Product deleted. Id: {Id}", request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
