namespace BizI.Application.Features.Warehouses.Delete;

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
