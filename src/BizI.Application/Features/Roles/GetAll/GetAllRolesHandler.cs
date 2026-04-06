using BizI.Application.Features.Roles.Dtos;

namespace BizI.Application.Features.Roles.GetAll;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRepository<Role> _repo;
    private readonly IMapper _mapper;

    public GetAllRolesHandler(IRepository<Role> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<RoleDto>>(await _repo.GetAllAsync());
}
