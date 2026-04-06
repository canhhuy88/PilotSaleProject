using BizI.Application.Features.Suppliers.Dtos;

namespace BizI.Application.Features.Suppliers.GetAll;

public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly IRepository<Supplier> _repo;
    private readonly IMapper _mapper;

    public GetAllSuppliersHandler(IRepository<Supplier> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<SupplierDto>>(await _repo.GetAllAsync());
}
