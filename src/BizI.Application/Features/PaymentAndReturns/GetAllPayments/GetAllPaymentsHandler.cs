using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetAllPayments;

public class GetAllPaymentsHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDto>>
{
    private readonly IRepository<Payment> _repo;
    private readonly IMapper _mapper;

    public GetAllPaymentsHandler(IRepository<Payment> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<PaymentDto>> Handle(GetAllPaymentsQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<PaymentDto>>(await _repo.GetAllAsync());
}
