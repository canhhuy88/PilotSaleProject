namespace BizI.Application.Features.Users.Create;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, CommandResult>
{
    private readonly IRepository<User> _repo;

    public CreateUserHandler(IRepository<User> repo) => _repo = repo;

    public async Task<CommandResult> Handle(CreateUserCommand r, CancellationToken ct)
    {
        try
        {
            // ✅ Domain factory — no direct property setters
            var user = User.Create(r.Username, r.PasswordHash, r.FullName, r.RoleId, r.BranchId);
            await _repo.AddAsync(user);
            return CommandResult.SuccessResult(user.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
