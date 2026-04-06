using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Inventory;

public record GetInventoryByIdQuery(string Id) : IRequest<BizI.Domain.Entities.Inventory?>;

public class GetInventoryByIdHandler : IRequestHandler<GetInventoryByIdQuery, BizI.Domain.Entities.Inventory?>
{
    private readonly IRepository<BizI.Domain.Entities.Inventory> _repo;

    public GetInventoryByIdHandler(IRepository<BizI.Domain.Entities.Inventory> repo)
    {
        _repo = repo;
    }

    public async Task<BizI.Domain.Entities.Inventory?> Handle(GetInventoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
