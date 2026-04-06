namespace BizI.Application.Features.Roles.Update;

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, CommandResult>
{
    private readonly IRepository<Role> _repo;

    public UpdateRoleHandler(IRepository<Role> repo) => _repo = repo;

    public async Task<CommandResult> Handle(UpdateRoleCommand r, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(r.Id);
        if (role is null) return CommandResult.FailureResult($"Role '{r.Id}' not found.");
        try
        {
            role.Rename(r.Name);
            if (r.Permissions is not null) role.SetPermissions(r.Permissions);
            await _repo.UpdateAsync(role);
            return CommandResult.SuccessResult(role.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
