using BizI.Application.Features.CustomerGroups.Dtos;

namespace BizI.Application.Features.CustomerGroups.GetAll;

public class GetAllCustomerGroupsHandler : IRequestHandler<GetAllCustomerGroupsQuery, IEnumerable<CustomerGroupDto>>
{
    private readonly IRepository<CustomerGroup> _repo;
    private readonly IMapper _mapper;

    public GetAllCustomerGroupsHandler(IRepository<CustomerGroup> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<CustomerGroupDto>> Handle(GetAllCustomerGroupsQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<CustomerGroupDto>>(await _repo.GetAllAsync());
}
