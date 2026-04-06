using BizI.Application.Features.Products.Dtos;

namespace BizI.Application.Features.Products.GetById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
