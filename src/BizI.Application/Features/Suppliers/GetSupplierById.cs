using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Suppliers;

public record GetSupplierByIdQuery(string Id) : IRequest<Supplier?>;

public class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, Supplier?>
{
    private readonly IRepository<Supplier> _repo;

    public GetSupplierByIdHandler(IRepository<Supplier> repo)
    {
        _repo = repo;
    }

    public async Task<Supplier?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
