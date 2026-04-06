namespace BizI.Application.Features.Users.Update;

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, CommandResult>
{
    private readonly IRepository<User> _repo;

    public UpdateUserHandler(IRepository<User> repo) => _repo = repo;

    public async Task<CommandResult> Handle(UpdateUserCommand r, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(r.Id);
        if (user is null) return CommandResult.FailureResult($"User '{r.Id}' not found.");
        try
        {
            user.UpdateProfile(r.FullName, r.BranchId);
            if (!string.IsNullOrWhiteSpace(r.NewPasswordHash))
                user.ChangePassword(r.NewPasswordHash);
            await _repo.UpdateAsync(user);
            return CommandResult.SuccessResult(user.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class AssignRoleHandler : IRequestHandler<AssignRoleCommand, CommandResult>
{
    private readonly IRepository<User> _repo;

    public AssignRoleHandler(IRepository<User> repo) => _repo = repo;

    public async Task<CommandResult> Handle(AssignRoleCommand r, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(r.UserId);
        if (user is null) return CommandResult.FailureResult($"User '{r.UserId}' not found.");
        try
        {
            user.AssignRole(r.RoleId);
            await _repo.UpdateAsync(user);
            return CommandResult.SuccessResult(user.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
