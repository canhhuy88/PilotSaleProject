using BizI.Application.Features.Categories.Dtos;

namespace BizI.Application.Features.Categories.GetById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly IRepository<Category> _repo;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(IRepository<Category> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetByIdAsync(request.Id);
        return category is null ? null : _mapper.Map<CategoryDto>(category);
    }
}
