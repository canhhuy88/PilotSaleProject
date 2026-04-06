using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ProductVariants;

public record GetProductVariantByIdQuery(string Id) : IRequest<ProductVariant?>;

public class GetProductVariantByIdHandler : IRequestHandler<GetProductVariantByIdQuery, ProductVariant?>
{
    private readonly IRepository<ProductVariant> _repo;

    public GetProductVariantByIdHandler(IRepository<ProductVariant> repo)
    {
        _repo = repo;
    }

    public async Task<ProductVariant?> Handle(GetProductVariantByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
