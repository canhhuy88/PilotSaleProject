namespace BizI.Application.Features.Suppliers.Create;

public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, CommandResult>
{
    private readonly IRepository<Supplier> _repo;

    public CreateSupplierHandler(IRepository<Supplier> repo) => _repo = repo;

    public async Task<CommandResult> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Domain factory
            var supplier = Supplier.Create(request.Name, request.Phone, request.Address);
            await _repo.AddAsync(supplier);
            return CommandResult.SuccessResult(supplier.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
