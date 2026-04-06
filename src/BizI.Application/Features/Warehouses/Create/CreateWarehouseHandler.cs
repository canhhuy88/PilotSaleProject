namespace BizI.Application.Features.Warehouses.Create;

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
