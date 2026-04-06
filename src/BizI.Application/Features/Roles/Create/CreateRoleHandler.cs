namespace BizI.Application.Features.Roles.Create;

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, CommandResult>
{
    private readonly IRepository<Role> _repo;

    public CreateRoleHandler(IRepository<Role> repo) => _repo = repo;

    public async Task<CommandResult> Handle(CreateRoleCommand r, CancellationToken ct)
    {
        try
        {
            var role = Role.Create(r.Name, r.Permissions);
            await _repo.AddAsync(role);
            return CommandResult.SuccessResult(role.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
