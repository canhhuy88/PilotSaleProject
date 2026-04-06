using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Roles;

public record GetAllRolesQuery() : IRequest<IEnumerable<Role>>;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<Role>>
{
    private readonly IRepository<Role> _repo;

    public GetAllRolesHandler(IRepository<Role> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
