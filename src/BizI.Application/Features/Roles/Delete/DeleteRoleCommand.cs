namespace BizI.Application.Features.Roles.Delete;

public record DeleteRoleCommand(Guid Id) : IRequest<CommandResult>;
