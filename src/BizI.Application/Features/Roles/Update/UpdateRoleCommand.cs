namespace BizI.Application.Features.Roles.Update;

public record UpdateRoleCommand(Guid Id, string Name, List<string>? Permissions = null) : IRequest<CommandResult>;
