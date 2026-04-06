using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.PaymentAndReturns;

public record GetDebtByIdQuery(string Id) : IRequest<Debt?>;

public class GetDebtByIdHandler : IRequestHandler<GetDebtByIdQuery, Debt?>
{
    private readonly IRepository<Debt> _repo;

    public GetDebtByIdHandler(IRepository<Debt> repo)
    {
        _repo = repo;
    }

    public async Task<Debt?> Handle(GetDebtByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
