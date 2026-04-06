using BizI.Application.Features.Suppliers.Dtos;

namespace BizI.Application.Features.Suppliers.GetById;

public class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto?>
{
    private readonly IRepository<Supplier> _repo;
    private readonly IMapper _mapper;

    public GetSupplierByIdHandler(IRepository<Supplier> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<SupplierDto?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var s = await _repo.GetByIdAsync(request.Id);
        return s is null ? null : _mapper.Map<SupplierDto>(s);
    }
}
