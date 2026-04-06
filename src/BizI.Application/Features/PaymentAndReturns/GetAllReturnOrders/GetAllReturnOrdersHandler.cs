using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetAllReturnOrders;

public class GetAllReturnOrdersHandler : IRequestHandler<GetAllReturnOrdersQuery, IEnumerable<ReturnOrderReadDto>>
{
    private readonly IRepository<ReturnOrder> _repo;
    private readonly IMapper _mapper;

    public GetAllReturnOrdersHandler(IRepository<ReturnOrder> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<ReturnOrderReadDto>> Handle(GetAllReturnOrdersQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<ReturnOrderReadDto>>(await _repo.GetAllAsync());
}
