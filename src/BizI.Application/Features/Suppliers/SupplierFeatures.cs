using BizI.Application.DTOs.Supplier;

namespace BizI.Application.Features.Suppliers;

public record CreateSupplierCommand(string Name, string? Phone = null, string? Address = null) : IRequest<CommandResult>;
public record UpdateSupplierCommand(string Id, string Name, string? Phone = null, string? Address = null) : IRequest<CommandResult>;
public record DeleteSupplierCommand(string Id) : IRequest<CommandResult>;
public record GetAllSuppliersQuery : IRequest<IEnumerable<SupplierDto>>;
public record GetSupplierByIdQuery(string Id) : IRequest<SupplierDto?>;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

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

public class GetAllSuppliersHandler : IRequestHandler<GetAllSuppliersQuery, IEnumerable<SupplierDto>>
{
    private readonly IRepository<Supplier> _repo;
    private readonly IMapper _mapper;
    public GetAllSuppliersHandler(IRepository<Supplier> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<SupplierDto>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<SupplierDto>>(await _repo.GetAllAsync());
}

public class GetSupplierByIdHandler : IRequestHandler<GetSupplierByIdQuery, SupplierDto?>
{
    private readonly IRepository<Supplier> _repo;
    private readonly IMapper _mapper;
    public GetSupplierByIdHandler(IRepository<Supplier> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<SupplierDto?> Handle(GetSupplierByIdQuery request, CancellationToken cancellationToken)
    {
        var s = await _repo.GetByIdAsync(request.Id);
        return s is null ? null : _mapper.Map<SupplierDto>(s);
    }
}
