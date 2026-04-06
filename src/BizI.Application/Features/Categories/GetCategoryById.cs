using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Categories;

public record GetCategoryByIdQuery(string Id) : IRequest<Category?>;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, Category?>
{
    private readonly IRepository<Category> _categoryRepo;

    public GetCategoryByIdHandler(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task<Category?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _categoryRepo.GetByIdAsync(request.Id);
    }
}
