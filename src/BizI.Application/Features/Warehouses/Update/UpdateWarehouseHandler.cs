namespace BizI.Application.Features.Warehouses.Update;

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
