using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products;

public record CreateProductCommand(string Name, decimal Price) : IRequest<CommandResult>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CommandResult>
{
    private readonly IRepository<Product> _productRepo;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(IRepository<Product> productRepo, ILogger<CreateProductHandler> logger)
    {
        _productRepo = productRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateProductCommand: {ProductName}, Price: {Price}", request.Name, request.Price);

        try
        {
            var product = new Product { Name = request.Name, SalePrice = request.Price };
            await _productRepo.AddAsync(product);

            _logger.LogInformation("Product created successfully with Id: {ProductId}", product.Id);

            return CommandResult.SuccessResult(product.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create product: {ProductName}", request.Name);
            throw;
        }
    }
}
