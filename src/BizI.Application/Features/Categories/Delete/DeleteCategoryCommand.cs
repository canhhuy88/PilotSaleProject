namespace BizI.Application.Features.Categories.Delete;

public record DeleteCategoryCommand(Guid Id) : IRequest<CommandResult>;
