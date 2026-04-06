namespace BizI.Application.Features.Suppliers.Update;

public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, CommandResult>
{
    private readonly IRepository<Supplier> _repo;

    public UpdateSupplierHandler(IRepository<Supplier> repo) => _repo = repo;

    public async Task<CommandResult> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        var supplier = await _repo.GetByIdAsync(request.Id);
        if (supplier is null) return CommandResult.FailureResult($"Supplier '{request.Id}' not found.");
        try
        {
            // ✅ Domain methods — no direct property mutation
            supplier.Rename(request.Name);
            if (!string.IsNullOrWhiteSpace(request.Phone)) supplier.ChangePhone(request.Phone);
            if (!string.IsNullOrWhiteSpace(request.Address)) supplier.ChangeAddress(request.Address);
            await _repo.UpdateAsync(supplier);
            return CommandResult.SuccessResult(supplier.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
