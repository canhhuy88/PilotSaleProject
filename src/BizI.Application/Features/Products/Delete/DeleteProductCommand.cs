namespace BizI.Application.Features.Products.Delete;

public record DeleteProductCommand(Guid Id) : IRequest<CommandResult>;
