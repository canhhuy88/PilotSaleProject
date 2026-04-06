using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders;

public record GetImportOrderByIdQuery(string Id) : IRequest<ImportOrder?>;

public class GetImportOrderByIdHandler : IRequestHandler<GetImportOrderByIdQuery, ImportOrder?>
{
    private readonly IRepository<ImportOrder> _repo;

    public GetImportOrderByIdHandler(IRepository<ImportOrder> repo)
    {
        _repo = repo;
    }

    public async Task<ImportOrder?> Handle(GetImportOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
