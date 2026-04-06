using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Roles;

public record GetRoleByIdQuery(string Id) : IRequest<Role?>;

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, Role?>
{
    private readonly IRepository<Role> _repo;

    public GetRoleByIdHandler(IRepository<Role> repo)
    {
        _repo = repo;
    }

    public async Task<Role?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
