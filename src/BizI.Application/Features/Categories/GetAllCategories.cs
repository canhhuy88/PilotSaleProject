using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Categories;

public record GetAllCategoriesQuery() : IRequest<IEnumerable<Category>>;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<Category>>
{
    private readonly IRepository<Category> _categoryRepo;

    public GetAllCategoriesHandler(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<IEnumerable<Category>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepo.GetAllAsync();
    }
}
