using BizI.Application.DTOs.Role;

namespace BizI.Application.Features.Roles;

public record CreateRoleCommand(string Name, List<string>? Permissions = null) : IRequest<CommandResult>;
public record UpdateRoleCommand(Guid Id, string Name, List<string>? Permissions = null) : IRequest<CommandResult>;
public record DeleteRoleCommand(Guid Id) : IRequest<CommandResult>;
public record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;
public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto?>;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator() { RuleFor(x => x.Name).NotEmpty().MaximumLength(100); }
}

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

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRepository<Role> _repo;
    private readonly IMapper _mapper;
    public GetAllRolesHandler(IRepository<Role> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<RoleDto>>(await _repo.GetAllAsync());
}

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRepository<Role> _repo;
    private readonly IMapper _mapper;
    public GetRoleByIdHandler(IRepository<Role> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<RoleDto?> Handle(GetRoleByIdQuery r, CancellationToken ct)
    {
        var role = await _repo.GetByIdAsync(r.Id);
        return role is null ? null : _mapper.Map<RoleDto>(role);
    }
}
