using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetAllDebts;

public class GetAllDebtsHandler : IRequestHandler<GetAllDebtsQuery, IEnumerable<DebtDto>>
{
    private readonly IRepository<Debt> _repo;
    private readonly IMapper _mapper;

    public GetAllDebtsHandler(IRepository<Debt> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<DebtDto>> Handle(GetAllDebtsQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<DebtDto>>(await _repo.GetAllAsync());
}
