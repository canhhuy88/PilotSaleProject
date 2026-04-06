namespace BizI.Application.Features.Products;

public record GetAllProductsQuery() : IRequest<IEnumerable<Product>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IRepository<Product> _productRepo;

    public GetAllProductsHandler(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        return await _productRepo.GetAllAsync();
    }
}
