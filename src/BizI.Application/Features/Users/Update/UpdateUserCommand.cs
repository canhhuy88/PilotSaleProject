namespace BizI.Application.Features.Users.Update;

public record UpdateUserCommand(Guid Id, string FullName, Guid BranchId, string? NewPasswordHash = null) : IRequest<CommandResult>;

public record AssignRoleCommand(Guid UserId, Guid RoleId) : IRequest<CommandResult>;
