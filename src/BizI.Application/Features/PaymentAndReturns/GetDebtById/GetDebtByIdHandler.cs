using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetDebtById;

public class GetDebtByIdHandler : IRequestHandler<GetDebtByIdQuery, DebtDto?>
{
    private readonly IRepository<Debt> _repo;
    private readonly IMapper _mapper;

    public GetDebtByIdHandler(IRepository<Debt> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<DebtDto?> Handle(GetDebtByIdQuery r, CancellationToken ct)
    {
        var d = await _repo.GetByIdAsync(r.Id);
        return d is null ? null : _mapper.Map<DebtDto>(d);
    }
}
