namespace BizI.Application.DTOs.Category;

public record CategoryDto(
    string Id,
    string Name,
    string? ParentId,
    string? Description);

public record CreateCategoryDto(string Name, string? ParentId = null, string? Description = null);

public record UpdateCategoryDto(string Id, string Name, string? Description = null);
