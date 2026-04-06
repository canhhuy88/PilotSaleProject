using BizI.Application.Features.Products.Dtos;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Products.GetAll;

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
