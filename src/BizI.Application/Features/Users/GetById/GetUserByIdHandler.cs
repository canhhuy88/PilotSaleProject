using BizI.Application.Features.Users.Dtos;

namespace BizI.Application.Features.Users.GetById;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IRepository<User> _repo;
    private readonly IMapper _mapper;

    public GetUserByIdHandler(IRepository<User> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<UserDto?> Handle(GetUserByIdQuery r, CancellationToken ct)
    {
        var u = await _repo.GetByIdAsync(r.Id);
        return u is null ? null : _mapper.Map<UserDto>(u);
    }
}
