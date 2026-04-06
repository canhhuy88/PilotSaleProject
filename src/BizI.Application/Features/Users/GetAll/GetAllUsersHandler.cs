using BizI.Application.Features.Users.Dtos;

namespace BizI.Application.Features.Users.GetAll;

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IRepository<User> _repo;
    private readonly IMapper _mapper;

    public GetAllUsersHandler(IRepository<User> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<UserDto>>(await _repo.GetAllAsync());
}
