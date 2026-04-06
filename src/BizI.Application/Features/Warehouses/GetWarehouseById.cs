using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Warehouses;

public record GetWarehouseByIdQuery(string Id) : IRequest<Warehouse?>;

public class GetWarehouseByIdHandler : IRequestHandler<GetWarehouseByIdQuery, Warehouse?>
{
    private readonly IRepository<Warehouse> _repo;

    public GetWarehouseByIdHandler(IRepository<Warehouse> repo)
    {
        _repo = repo;
    }

    public async Task<Warehouse?> Handle(GetWarehouseByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
