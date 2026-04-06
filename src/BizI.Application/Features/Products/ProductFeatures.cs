using BizI.Application.DTOs.Product;
using BizI.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products;

// ── Commands/Queries ─────────────────────────────────────────────────────────

public record CreateProductCommand(
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    Guid CategoryId,
    string? Description = null,
    string? Barcode = null,
    string Currency = "VND") : IRequest<CommandResult>;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    Guid CategoryId,
    string? Description = null,
    string? Barcode = null,
    string Currency = "VND") : IRequest<CommandResult>;

public record DeleteProductCommand(Guid Id) : IRequest<CommandResult>;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

// ── Validators ───────────────────────────────────────────────────────────────

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SKU).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Unit).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SKU).NotEmpty().MaximumLength(100);
        RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Unit).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}

// ── Handlers ─────────────────────────────────────────────────────────────────

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

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsHandler> _logger;

    public GetAllProductsHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetAllProductsHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching all products.");
        var products = await _productRepository.GetAllAsync();

        // ✅ Map entities → DTOs; API never receives raw entities
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }
}

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductByIdHandler> _logger;

    public GetProductByIdHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<GetProductByIdHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Fetching product by id: {Id}", request.Id);
        var product = await _productRepository.GetByIdAsync(request.Id);

        if (product is null)
        {
            _logger.LogWarning("Product not found. Id: {Id}", request.Id);
            return null;
        }

        return _mapper.Map<ProductDto>(product);
    }
}
