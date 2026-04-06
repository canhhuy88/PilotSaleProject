namespace BizI.Application.Features.Users.Delete;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, CommandResult>
{
    private readonly IRepository<User> _repo;

    public DeleteUserHandler(IRepository<User> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteUserCommand r, CancellationToken ct)
    {
        var user = await _repo.GetByIdAsync(r.Id);
        if (user is null) return CommandResult.FailureResult($"User '{r.Id}' not found.");
        user.Deactivate();
        await _repo.UpdateAsync(user);
        return CommandResult.SuccessResult(r.Id);
    }
}
