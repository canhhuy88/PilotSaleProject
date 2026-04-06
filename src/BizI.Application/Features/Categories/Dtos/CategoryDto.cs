namespace BizI.Application.Features.Categories.Dtos;

public record CategoryDto(
    Guid Id,
    string Name,
    Guid? ParentId,
    string? Description);

public record CreateCategoryDto(string Name, Guid? ParentId = null, string? Description = null);

public record UpdateCategoryDto(Guid Id, string Name, string? Description = null);
