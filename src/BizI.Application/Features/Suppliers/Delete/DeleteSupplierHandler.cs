namespace BizI.Application.Features.Suppliers.Delete;

public class DeleteSupplierHandler : IRequestHandler<DeleteSupplierCommand, CommandResult>
{
    private readonly IRepository<Supplier> _repo;

    public DeleteSupplierHandler(IRepository<Supplier> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repo.GetByIdAsync(request.Id);
        if (supplier is null) return CommandResult.FailureResult($"Supplier '{request.Id}' not found.");
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
