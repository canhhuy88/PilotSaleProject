using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.Orders;

public record GetAllOrdersQuery() : IRequest<IEnumerable<Order>>;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<Order>>
{
    private readonly IRepository<Order> _repo;

    public GetAllOrdersHandler(IRepository<Order> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Order>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
