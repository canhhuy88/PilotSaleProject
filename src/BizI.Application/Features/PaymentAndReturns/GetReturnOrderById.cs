using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.PaymentAndReturns;

public record GetReturnOrderByIdQuery(string Id) : IRequest<ReturnOrder?>;

public class GetReturnOrderByIdHandler : IRequestHandler<GetReturnOrderByIdQuery, ReturnOrder?>
{
    private readonly IRepository<ReturnOrder> _repo;

    public GetReturnOrderByIdHandler(IRepository<ReturnOrder> repo)
    {
        _repo = repo;
    }

    public async Task<ReturnOrder?> Handle(GetReturnOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
