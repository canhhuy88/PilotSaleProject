using BizI.Application.Features.Products.Dtos;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products.GetById;

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
