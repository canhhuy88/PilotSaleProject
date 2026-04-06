using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products.Update;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, CommandResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(IProductRepository productRepository, ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating product. Id: {Id}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product is null)
            return CommandResult.FailureResult($"Product '{request.Id}' not found.");

        try
        {
            // ✅ Call domain method instead of setting properties directly
            product.UpdateDetails(
                request.Name, request.SKU,
                request.CostPrice, request.SalePrice,
                request.Unit, request.CategoryId,
                request.Description, request.Barcode,
                request.Currency);

            await _productRepository.UpdateAsync(product);
            _logger.LogInformation("Product updated. Id: {Id}", request.Id);
            return CommandResult.SuccessResult(product.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated while updating product {Id}: {Message}", request.Id, ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}
