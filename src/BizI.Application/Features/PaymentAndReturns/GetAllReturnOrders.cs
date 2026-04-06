using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.PaymentAndReturns;

public record GetAllReturnOrdersQuery() : IRequest<IEnumerable<ReturnOrder>>;

public class GetAllReturnOrdersHandler : IRequestHandler<GetAllReturnOrdersQuery, IEnumerable<ReturnOrder>>
{
    private readonly IRepository<ReturnOrder> _repo;

    public GetAllReturnOrdersHandler(IRepository<ReturnOrder> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ReturnOrder>> Handle(GetAllReturnOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
