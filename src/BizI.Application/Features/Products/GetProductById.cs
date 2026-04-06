using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Products;

public record GetProductByIdQuery(string Id) : IRequest<Product?>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IRepository<Product> _repo;

    public GetProductByIdHandler(IRepository<Product> repo)
    {
        _repo = repo;
    }

    public async Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
