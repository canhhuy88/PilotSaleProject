using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Users;

public record GetUserByIdQuery(string Id) : IRequest<User?>;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IRepository<User> _repo;

    public GetUserByIdHandler(IRepository<User> repo)
    {
        _repo = repo;
    }

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
