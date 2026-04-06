using BizI.Application.Features.Categories.Dtos;

namespace BizI.Application.Features.Categories.GetAll;

public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;
