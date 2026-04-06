using BizI.Application.DTOs.Inventory;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Inventory;

// ── Commands/Queries ─────────────────────────────────────────────────────────

public record ImportStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null) : IRequest<CommandResult>;

public record ExportStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null) : IRequest<CommandResult>;

public record ReturnStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity,
    Guid? ReferenceId = null) : IRequest<CommandResult>;

public record AdjustStockCommand(
    Guid ProductId,
    Guid WarehouseId,
    int NewQuantity) : IRequest<CommandResult>;

public record GetAllInventoryQuery : IRequest<IEnumerable<InventoryDto>>;

public record GetInventoryByProductQuery(Guid ProductId) : IRequest<IEnumerable<InventoryDto>>;

public record GetInventoryByWarehouseQuery(Guid WarehouseId) : IRequest<IEnumerable<InventoryDto>>;

// ── Validators ───────────────────────────────────────────────────────────────

public class ImportStockCommandValidator : AbstractValidator<ImportStockCommand>
{
    public ImportStockCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}

public class AdjustStockCommandValidator : AbstractValidator<AdjustStockCommand>
{
    public AdjustStockCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.NewQuantity).GreaterThanOrEqualTo(0);
    }
}

// ── Handlers ─────────────────────────────────────────────────────────────────

public class ImportStockHandler : IRequestHandler<ImportStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ImportStockHandler(IInventoryService inventoryService)
        => _inventoryService = inventoryService;

    public async Task<CommandResult> Handle(ImportStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.ImportStockAsync(
                request.ProductId, request.WarehouseId,
                request.Quantity, request.ReferenceId);
            return CommandResult.SuccessResult();
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class ExportStockHandler : IRequestHandler<ExportStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ExportStockHandler(IInventoryService inventoryService)
        => _inventoryService = inventoryService;

    public async Task<CommandResult> Handle(ExportStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.ExportStockAsync(
                request.ProductId, request.WarehouseId,
                request.Quantity, request.ReferenceId);
            return CommandResult.SuccessResult();
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class ReturnStockHandler : IRequestHandler<ReturnStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public ReturnStockHandler(IInventoryService inventoryService)
        => _inventoryService = inventoryService;

    public async Task<CommandResult> Handle(ReturnStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.ReturnStockAsync(
                request.ProductId, request.WarehouseId,
                request.Quantity, request.ReferenceId);
            return CommandResult.SuccessResult();
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class AdjustStockHandler : IRequestHandler<AdjustStockCommand, CommandResult>
{
    private readonly IInventoryService _inventoryService;

    public AdjustStockHandler(IInventoryService inventoryService)
        => _inventoryService = inventoryService;

    public async Task<CommandResult> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _inventoryService.AdjustStockAsync(
                request.ProductId, request.WarehouseId, request.NewQuantity);
            return CommandResult.SuccessResult();
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class GetAllInventoryHandler : IRequestHandler<GetAllInventoryQuery, IEnumerable<InventoryDto>>
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IMapper _mapper;

    public GetAllInventoryHandler(IInventoryRepository inventoryRepo, IMapper mapper)
    {
        _inventoryRepo = inventoryRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetAllInventoryQuery request, CancellationToken cancellationToken)
    {
        var all = await _inventoryRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<InventoryDto>>(all);
    }
}

public class GetInventoryByProductHandler : IRequestHandler<GetInventoryByProductQuery, IEnumerable<InventoryDto>>
{
    private readonly IInventoryRepository _inventoryRepo;
    private readonly IMapper _mapper;

    public GetInventoryByProductHandler(IInventoryRepository inventoryRepo, IMapper mapper)
    {
        _inventoryRepo = inventoryRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<InventoryDto>> Handle(GetInventoryByProductQuery request, CancellationToken cancellationToken)
    {
        var productGuid = request.ProductId;
        var records = await _inventoryRepo.GetByProductAsync(productGuid);
        return _mapper.Map<IEnumerable<InventoryDto>>(records);
    }
}
