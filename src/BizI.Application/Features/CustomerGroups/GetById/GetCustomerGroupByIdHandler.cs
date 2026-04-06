using BizI.Application.Features.CustomerGroups.Dtos;

namespace BizI.Application.Features.CustomerGroups.GetById;

public class GetCustomerGroupByIdHandler : IRequestHandler<GetCustomerGroupByIdQuery, CustomerGroupDto?>
{
    private readonly IRepository<CustomerGroup> _repo;
    private readonly IMapper _mapper;

    public GetCustomerGroupByIdHandler(IRepository<CustomerGroup> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<CustomerGroupDto?> Handle(GetCustomerGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await _repo.GetByIdAsync(request.Id);
        return group is null ? null : _mapper.Map<CustomerGroupDto>(group);
    }
}
