using BizI.Application.Features.Categories.Dtos;

namespace BizI.Application.Features.Categories.GetById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto?>;
