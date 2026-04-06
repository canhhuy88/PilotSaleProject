using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.PaymentAndReturns;

public record GetAllPaymentsQuery() : IRequest<IEnumerable<Payment>>;

public class GetAllPaymentsHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<Payment>>
{
    private readonly IRepository<Payment> _repo;

    public GetAllPaymentsHandler(IRepository<Payment> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Payment>> Handle(GetAllPaymentsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
