using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Suppliers;

public record GetAllSuppliersQuery() : IRequest<IEnumerable<Supplier>>;

public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<Supplier>>
{
    private readonly IRepository<Supplier> _repo;

    public GetAllSuppliersHandler(IRepository<Supplier> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Supplier>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
