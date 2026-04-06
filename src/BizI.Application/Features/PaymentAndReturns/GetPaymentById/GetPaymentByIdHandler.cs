using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetPaymentById;

public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
{
    private readonly IRepository<Payment> _repo;
    private readonly IMapper _mapper;

    public GetPaymentByIdHandler(IRepository<Payment> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<PaymentDto?> Handle(GetPaymentByIdQuery r, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(r.Id);
        return p is null ? null : _mapper.Map<PaymentDto>(p);
    }
}
