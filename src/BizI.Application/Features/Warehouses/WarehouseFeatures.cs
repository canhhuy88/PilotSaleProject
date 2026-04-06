using BizI.Application.DTOs.Warehouse;

namespace BizI.Application.Features.Warehouses;

public record CreateWarehouseCommand(string Name, string BranchId) : IRequest<CommandResult>;
public record UpdateWarehouseCommand(string Id, string Name, string BranchId) : IRequest<CommandResult>;
public record DeleteWarehouseCommand(string Id) : IRequest<CommandResult>;
public record GetAllWarehousesQuery : IRequest<IEnumerable<WarehouseDto>>;
public record GetWarehouseByIdQuery(string Id) : IRequest<WarehouseDto?>;

public class CreateWarehouseCommandValidator : AbstractValidator<CreateWarehouseCommand>
{
    public CreateWarehouseCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.BranchId).NotEmpty();
    }
}

public class CreateWarehouseHandler : IRequestHandler<CreateWarehouseCommand, CommandResult>
{
    private readonly IRepository<Warehouse> _repo;
    public CreateWarehouseHandler(IRepository<Warehouse> repo) => _repo = repo;
    public async Task<CommandResult> Handle(CreateWarehouseCommand r, CancellationToken ct)
    {
        try
        {
            var w = Warehouse.Create(r.Name, r.BranchId);
            await _repo.AddAsync(w);
            return CommandResult.SuccessResult(w.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class UpdateWarehouseHandler : IRequestHandler<UpdateWarehouseCommand, CommandResult>
{
    private readonly IRepository<Warehouse> _repo;
    public UpdateWarehouseHandler(IRepository<Warehouse> repo) => _repo = repo;
    public async Task<CommandResult> Handle(UpdateWarehouseCommand r, CancellationToken ct)
    {
        var w = await _repo.GetByIdAsync(r.Id);
        if (w is null) return CommandResult.FailureResult($"Warehouse '{r.Id}' not found.");
        try
        {
            w.Rename(r.Name);
            w.Reassign(r.BranchId);
            await _repo.UpdateAsync(w);
            return CommandResult.SuccessResult(w.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class DeleteWarehouseHandler : IRequestHandler<DeleteWarehouseCommand, CommandResult>
{
    private readonly IRepository<Warehouse> _repo;
    public DeleteWarehouseHandler(IRepository<Warehouse> repo) => _repo = repo;
    public async Task<CommandResult> Handle(DeleteWarehouseCommand r, CancellationToken ct)
    {
        var w = await _repo.GetByIdAsync(r.Id);
        if (w is null) return CommandResult.FailureResult($"Warehouse '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}

public class GetAllWarehousesHandler : IRequestHandler<GetAllWarehousesQuery, IEnumerable<WarehouseDto>>
{
    private readonly IRepository<Warehouse> _repo;
    private readonly IMapper _mapper;
    public GetAllWarehousesHandler(IRepository<Warehouse> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<WarehouseDto>> Handle(GetAllWarehousesQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<WarehouseDto>>(await _repo.GetAllAsync());
}

public class GetWarehouseByIdHandler : IRequestHandler<GetWarehouseByIdQuery, WarehouseDto?>
{
    private readonly IRepository<Warehouse> _repo;
    private readonly IMapper _mapper;
    public GetWarehouseByIdHandler(IRepository<Warehouse> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<WarehouseDto?> Handle(GetWarehouseByIdQuery r, CancellationToken ct)
    {
        var w = await _repo.GetByIdAsync(r.Id);
        return w is null ? null : _mapper.Map<WarehouseDto>(w);
    }
}
