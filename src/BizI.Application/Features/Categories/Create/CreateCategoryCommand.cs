namespace BizI.Application.Features.Categories.Create;

public record CreateCategoryCommand(
    string Name,
    Guid? ParentId = null,
    string? Description = null) : IRequest<CommandResult>;
