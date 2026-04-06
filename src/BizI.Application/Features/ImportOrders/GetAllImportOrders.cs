using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders;

public record GetAllImportOrdersQuery() : IRequest<IEnumerable<ImportOrder>>;

public class GetAllImportOrdersHandler : IRequestHandler<GetAllImportOrdersQuery, IEnumerable<ImportOrder>>
{
    private readonly IRepository<ImportOrder> _repo;

    public GetAllImportOrdersHandler(IRepository<ImportOrder> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ImportOrder>> Handle(GetAllImportOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
