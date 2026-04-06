using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Users;

public record GetAllUsersQuery() : IRequest<IEnumerable<User>>;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<User>>
{
    private readonly IRepository<User> _repo;

    public GetAllUsersHandler(IRepository<User> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
