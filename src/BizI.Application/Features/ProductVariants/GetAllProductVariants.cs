using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.ProductVariants;

public record GetAllProductVariantsQuery() : IRequest<IEnumerable<ProductVariant>>;

public class GetAllProductVariantsHandler : IRequestHandler<GetAllProductVariantsQuery, IEnumerable<ProductVariant>>
{
    private readonly IRepository<ProductVariant> _repo;

    public GetAllProductVariantsHandler(IRepository<ProductVariant> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ProductVariant>> Handle(GetAllProductVariantsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
