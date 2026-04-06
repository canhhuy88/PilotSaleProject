using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.PaymentAndReturns;

public record GetPaymentByIdQuery(string Id) : IRequest<Payment?>;

public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, Payment?>
{
    private readonly IRepository<Payment> _repo;

    public GetPaymentByIdHandler(IRepository<Payment> repo)
    {
        _repo = repo;
    }

    public async Task<Payment?> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
