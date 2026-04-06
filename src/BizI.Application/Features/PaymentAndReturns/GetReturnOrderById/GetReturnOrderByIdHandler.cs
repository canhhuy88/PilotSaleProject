using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetReturnOrderById;

public class GetReturnOrderByIdHandler : IRequestHandler<GetReturnOrderByIdQuery, ReturnOrderReadDto?>
{
    private readonly IRepository<ReturnOrder> _repo;
    private readonly IMapper _mapper;

    public GetReturnOrderByIdHandler(IRepository<ReturnOrder> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<ReturnOrderReadDto?> Handle(GetReturnOrderByIdQuery r, CancellationToken ct)
    {
        var ro = await _repo.GetByIdAsync(r.Id);
        return ro is null ? null : _mapper.Map<ReturnOrderReadDto>(ro);
    }
}
