namespace BizI.Application.Features.Users.Create;

public record CreateUserCommand(
    string Username,
    string PasswordHash,
    string FullName,
    Guid RoleId,
    Guid BranchId = default) : IRequest<CommandResult>;
