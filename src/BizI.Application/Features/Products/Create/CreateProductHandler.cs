using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products.Create;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CommandResult>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(IProductRepository productRepository, ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating product. SKU: {SKU}, Name: {Name}", request.SKU, request.Name);

        var existing = await _productRepository.GetBySkuAsync(request.SKU);
        if (existing is not null)
        {
            _logger.LogWarning("Duplicate SKU detected: {SKU}", request.SKU);
            return CommandResult.FailureResult($"A product with SKU '{request.SKU}' already exists.");
        }

        try
        {
            // ✅ Use Domain factory — respects encapsulation
            var product = Product.Create(
                request.Name, request.SKU,
                request.CostPrice, request.SalePrice,
                request.Unit, request.CategoryId,
                request.Description, request.Barcode,
                request.Currency);

            await _productRepository.AddAsync(product);

            _logger.LogInformation("Product created. Id: {ProductId}, SKU: {SKU}", product.Id, product.SKU);
            return CommandResult.SuccessResult(product.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated while creating product: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}
