using BizI.Application.Features.Roles.Dtos;

namespace BizI.Application.Features.Roles.GetById;

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRepository<Role> _repo;
    private readonly IMapper _mapper;

    public GetRoleByIdHandler(IRepository<Role> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery r, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(r.Id);
        return role is null ? null : _mapper.Map<RoleDto>(role);
    }
}
