namespace BizI.Application.Features.Roles.Create;

public record CreateRoleCommand(string Name, List<string>? Permissions = null) : IRequest<CommandResult>;
