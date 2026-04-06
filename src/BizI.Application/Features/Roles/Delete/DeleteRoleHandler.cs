namespace BizI.Application.Features.Roles.Delete;

public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, CommandResult>
{
    private readonly IRepository<Role> _repo;

    public DeleteRoleHandler(IRepository<Role> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteRoleCommand r, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(r.Id);
        if (role is null) return CommandResult.FailureResult($"Role '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}
