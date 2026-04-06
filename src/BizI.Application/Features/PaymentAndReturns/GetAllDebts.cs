using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.PaymentAndReturns;

public record GetAllDebtsQuery() : IRequest<IEnumerable<Debt>>;

public class GetAllDebtsHandler : IRequestHandler<GetAllDebtsQuery, IEnumerable<Debt>>
{
    private readonly IRepository<Debt> _repo;

    public GetAllDebtsHandler(IRepository<Debt> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Debt>> Handle(GetAllDebtsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
