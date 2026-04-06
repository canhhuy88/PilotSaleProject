using BizI.Application.Features.Products.Dtos;

namespace BizI.Application.Features.Products.GetAll;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;
