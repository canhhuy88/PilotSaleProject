namespace BizI.Application.Features.Users.Delete;

public record DeleteUserCommand(Guid Id) : IRequest<CommandResult>;
