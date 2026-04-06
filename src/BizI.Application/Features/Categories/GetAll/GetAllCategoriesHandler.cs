using BizI.Application.Features.Categories.Dtos;

namespace BizI.Application.Features.Categories.GetAll;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IRepository<Category> _repo;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetAllCategoriesHandler(IRepository<Category> repo, IMapper mapper, ICurrentUserService currentUser)
    {
        _repo = repo;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUser.Username;
        var all = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(all);
    }
}
