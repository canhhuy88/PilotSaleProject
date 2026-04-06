using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Warehouses;

public record GetAllWarehousesQuery() : IRequest<IEnumerable<Warehouse>>;

public class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesQuery, IEnumerable<Warehouse>>
{
    private readonly IRepository<Warehouse> _repo;

    public GetAllWarehousesHandler(IRepository<Warehouse> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Warehouse>> Handle(GetAllWarehousesQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
