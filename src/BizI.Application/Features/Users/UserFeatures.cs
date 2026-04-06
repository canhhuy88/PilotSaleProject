using BizI.Application.DTOs.User;

namespace BizI.Application.Features.Users;

public record CreateUserCommand(
    string Username,
    string PasswordHash,
    string FullName,
    Guid RoleId,
    Guid BranchId = default) : IRequest<CommandResult>;

public record UpdateUserCommand(Guid Id, string FullName, Guid BranchId, string? NewPasswordHash = null) : IRequest<CommandResult>;
public record AssignRoleCommand(Guid UserId, Guid RoleId) : IRequest<CommandResult>;
public record DeleteUserCommand(Guid Id) : IRequest<CommandResult>;
public record GetAllUsersQuery : IRequest<IEnumerable<UserDto>>;
public record GetUserByIdQuery(Guid Id) : IRequest<UserDto?>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PasswordHash).NotEmpty();
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.RoleId).NotEmpty();
    }
}

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

public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IRepository<User> _repo;
    private readonly IMapper _mapper;
    public GetAllUsersHandler(IRepository<User> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<UserDto>> Handle(GetAllUsersQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<UserDto>>(await _repo.GetAllAsync());
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IRepository<User> _repo;
    private readonly IMapper _mapper;
    public GetUserByIdHandler(IRepository<User> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<UserDto?> Handle(GetUserByIdQuery r, CancellationToken ct)
    {
        var u = await _repo.GetByIdAsync(r.Id);
        return u is null ? null : _mapper.Map<UserDto>(u);
    }
}
