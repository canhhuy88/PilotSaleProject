namespace BizI.Application.Features.Categories.Update;

public record UpdateCategoryCommand(
    Guid Id,
    string Name,
    string? Description = null) : IRequest<CommandResult>;
