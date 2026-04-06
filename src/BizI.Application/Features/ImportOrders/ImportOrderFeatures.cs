using BizI.Application.DTOs.ImportOrder;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.ImportOrders;

public record CreateImportOrderCommand(
    Guid SupplierId,
    List<CreateImportOrderItemDto> Items) : IRequest<CommandResult>;

public record ConfirmImportOrderCommand(Guid Id) : IRequest<CommandResult>;
public record ReceiveImportOrderCommand(Guid Id) : IRequest<CommandResult>;
public record CancelImportOrderCommand(Guid Id) : IRequest<CommandResult>;
public record DeleteImportOrderCommand(Guid Id) : IRequest<CommandResult>;
public record GetAllImportOrdersQuery : IRequest<IEnumerable<ImportOrderDto>>;
public record GetImportOrderByIdQuery(Guid Id) : IRequest<ImportOrderDto?>;

public class CreateImportOrderCommandValidator : AbstractValidator<CreateImportOrderCommand>
{
    public CreateImportOrderCommandValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty().WithMessage("Import order must have at least one item.");
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
            item.RuleFor(x => x.CostPrice).GreaterThanOrEqualTo(0);
        });
    }
}

public class CreateImportOrderHandler : IRequestHandler<CreateImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly IInventoryService _inventoryService;
    private readonly ILogger<CreateImportOrderHandler> _logger;

    public CreateImportOrderHandler(
        IRepository<ImportOrder> repo,
        IInventoryService inventoryService,
        ILogger<CreateImportOrderHandler> logger)
    {
        _repo = repo;
        _inventoryService = inventoryService;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateImportOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Build domain child entities via factory
            var items = request.Items
                .Select(i => ImportOrderItem.Create(i.ProductId, i.Quantity, i.CostPrice, i.Currency))
                .ToList();

            // ✅ Domain factory — enforces business rules
            var importOrder = ImportOrder.Create(request.SupplierId, items);

            await _repo.AddAsync(importOrder);
            _logger.LogInformation("ImportOrder created. Id: {Id}", importOrder.Id);
            return CommandResult.SuccessResult(importOrder.Id);
        }
        catch (DomainException ex)
        {
            return CommandResult.FailureResult(ex.Message);
        }
    }
}

public class ConfirmImportOrderHandler : IRequestHandler<ConfirmImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    public ConfirmImportOrderHandler(IRepository<ImportOrder> repo) => _repo = repo;

    public async Task<CommandResult> Handle(ConfirmImportOrderCommand request, CancellationToken cancellationToken)
    {
        var io = await _repo.GetByIdAsync(request.Id);
        if (io is null) return CommandResult.FailureResult($"ImportOrder '{request.Id}' not found.");
        try
        {
            io.Confirm();  // ✅ Domain method
            await _repo.UpdateAsync(io);
            return CommandResult.SuccessResult(io.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class ReceiveImportOrderHandler : IRequestHandler<ReceiveImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly IInventoryService _inventoryService;

    public ReceiveImportOrderHandler(IRepository<ImportOrder> repo, IInventoryService inventoryService)
    {
        _repo = repo;
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(ReceiveImportOrderCommand request, CancellationToken cancellationToken)
    {
        var io = await _repo.GetByIdAsync(request.Id);
        if (io is null) return CommandResult.FailureResult($"ImportOrder '{request.Id}' not found.");
        try
        {
            io.MarkReceived();  // ✅ Domain method
            await _repo.UpdateAsync(io);
            return CommandResult.SuccessResult(io.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class DeleteImportOrderHandler : IRequestHandler<DeleteImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    public DeleteImportOrderHandler(IRepository<ImportOrder> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteImportOrderCommand request, CancellationToken cancellationToken)
    {
        var io = await _repo.GetByIdAsync(request.Id);
        if (io is null) return CommandResult.FailureResult($"ImportOrder '{request.Id}' not found.");
        try
        {
            io.Cancel();  // ✅ Domain method enforces cancellation rules
            await _repo.UpdateAsync(io);
            return CommandResult.SuccessResult(io.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class GetAllImportOrdersHandler : IRequestHandler<GetAllImportOrdersQuery, IEnumerable<ImportOrderDto>>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly IMapper _mapper;
    public GetAllImportOrdersHandler(IRepository<ImportOrder> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<ImportOrderDto>> Handle(GetAllImportOrdersQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<ImportOrderDto>>(await _repo.GetAllAsync());
}

public class GetImportOrderByIdHandler : IRequestHandler<GetImportOrderByIdQuery, ImportOrderDto?>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly IMapper _mapper;
    public GetImportOrderByIdHandler(IRepository<ImportOrder> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<ImportOrderDto?> Handle(GetImportOrderByIdQuery r, CancellationToken ct)
    {
        var io = await _repo.GetByIdAsync(r.Id);
        return io is null ? null : _mapper.Map<ImportOrderDto>(io);
    }
}
