using BizI.Application.DTOs.Payment;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.PaymentAndReturns;

// ── Commands/Queries ─────────────────────────────────────────────────────────

public record CreatePaymentCommand(Guid OrderId, decimal Amount, string Method, string Currency = "VND") : IRequest<CommandResult>;
public record CreateDebtCommand(Guid CustomerId, Guid OrderId, decimal Amount, string Currency = "VND") : IRequest<CommandResult>;
public record RecordDebtPaymentCommand(Guid DebtId, decimal PaidAmount, string Currency = "VND") : IRequest<CommandResult>;
public record CreateReturnOrderCommand(Guid OrderId, Guid WarehouseId, List<CreateReturnItemDto> Items, string Currency = "VND") : IRequest<CommandResult>;
public record DeletePaymentCommand(Guid Id) : IRequest<CommandResult>;
public record DeleteDebtCommand(Guid Id) : IRequest<CommandResult>;
public record DeleteReturnOrderCommand(Guid Id) : IRequest<CommandResult>;
public record GetAllPaymentsQuery : IRequest<IEnumerable<PaymentDto>>;
public record GetAllDebtsQuery : IRequest<IEnumerable<DebtDto>>;
public record GetAllReturnOrdersQuery : IRequest<IEnumerable<ReturnOrderReadDto>>;
public record GetPaymentByIdQuery(Guid Id) : IRequest<PaymentDto?>;
public record GetDebtByIdQuery(Guid Id) : IRequest<DebtDto?>;
public record GetReturnOrderByIdQuery(Guid Id) : IRequest<ReturnOrderReadDto?>;

// ── Validators ───────────────────────────────────────────────────────────────

public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Method).NotEmpty();
    }
}

public class CreateDebtCommandValidator : AbstractValidator<CreateDebtCommand>
{
    public CreateDebtCommandValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}

public class CreateReturnOrderCommandValidator : AbstractValidator<CreateReturnOrderCommand>
{
    public CreateReturnOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).NotEmpty();
            i.RuleFor(x => x.Quantity).GreaterThan(0);
            i.RuleFor(x => x.RefundPrice).GreaterThanOrEqualTo(0);
        });
    }
}

// ── Payment Handlers ─────────────────────────────────────────────────────────

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _paymentRepo;
    private readonly ILogger<CreatePaymentHandler> _logger;

    public CreatePaymentHandler(IRepository<Payment> paymentRepo, ILogger<CreatePaymentHandler> logger)
    {
        _paymentRepo = paymentRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Domain factory
            var payment = Payment.Create(request.OrderId, request.Amount, request.Method, request.Currency);
            await _paymentRepo.AddAsync(payment);
            _logger.LogInformation("Payment created. Id: {Id}", payment.Id);
            return CommandResult.SuccessResult(payment.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _repo;
    public DeletePaymentHandler(IRepository<Payment> repo) => _repo = repo;
    public async Task<CommandResult> Handle(DeletePaymentCommand r, CancellationToken ct)
    {
        var payment = await _repo.GetByIdAsync(r.Id);
        if (payment is null) return CommandResult.FailureResult($"Payment '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}

public class GetAllPaymentsHandler : IRequestHandler<GetAllPaymentsQuery, IEnumerable<PaymentDto>>
{
    private readonly IRepository<Payment> _repo;
    private readonly IMapper _mapper;
    public GetAllPaymentsHandler(IRepository<Payment> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<PaymentDto>> Handle(GetAllPaymentsQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<PaymentDto>>(await _repo.GetAllAsync());
}

public class GetPaymentByIdHandler : IRequestHandler<GetPaymentByIdQuery, PaymentDto?>
{
    private readonly IRepository<Payment> _repo;
    private readonly IMapper _mapper;
    public GetPaymentByIdHandler(IRepository<Payment> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<PaymentDto?> Handle(GetPaymentByIdQuery r, CancellationToken ct)
    {
        var p = await _repo.GetByIdAsync(r.Id);
        return p is null ? null : _mapper.Map<PaymentDto>(p);
    }
}

// ── Debt Handlers ─────────────────────────────────────────────────────────────

public class CreateDebtHandler : IRequestHandler<CreateDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;
    public CreateDebtHandler(IRepository<Debt> repo) => _repo = repo;
    public async Task<CommandResult> Handle(CreateDebtCommand r, CancellationToken ct)
    {
        try
        {
            var debt = Debt.Create(r.CustomerId, r.OrderId, r.Amount, r.Currency);
            await _repo.AddAsync(debt);
            return CommandResult.SuccessResult(debt.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class RecordDebtPaymentHandler : IRequestHandler<RecordDebtPaymentCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;
    public RecordDebtPaymentHandler(IRepository<Debt> repo) => _repo = repo;
    public async Task<CommandResult> Handle(RecordDebtPaymentCommand r, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(r.DebtId);
        if (debt is null) return CommandResult.FailureResult($"Debt '{r.DebtId}' not found.");
        try
        {
            debt.RecordPayment(r.PaidAmount, r.Currency);  // ✅ Domain method
            await _repo.UpdateAsync(debt);
            return CommandResult.SuccessResult(debt.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class DeleteDebtHandler : IRequestHandler<DeleteDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;
    public DeleteDebtHandler(IRepository<Debt> repo) => _repo = repo;
    public async Task<CommandResult> Handle(DeleteDebtCommand r, CancellationToken ct)
    {
        var debt = await _repo.GetByIdAsync(r.Id);
        if (debt is null) return CommandResult.FailureResult($"Debt '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}

public class GetAllDebtsHandler : IRequestHandler<GetAllDebtsQuery, IEnumerable<DebtDto>>
{
    private readonly IRepository<Debt> _repo;
    private readonly IMapper _mapper;
    public GetAllDebtsHandler(IRepository<Debt> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<DebtDto>> Handle(GetAllDebtsQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<DebtDto>>(await _repo.GetAllAsync());
}

public class GetDebtByIdHandler : IRequestHandler<GetDebtByIdQuery, DebtDto?>
{
    private readonly IRepository<Debt> _repo;
    private readonly IMapper _mapper;
    public GetDebtByIdHandler(IRepository<Debt> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<DebtDto?> Handle(GetDebtByIdQuery r, CancellationToken ct)
    {
        var d = await _repo.GetByIdAsync(r.Id);
        return d is null ? null : _mapper.Map<DebtDto>(d);
    }
}

// ── ReturnOrder Handlers ──────────────────────────────────────────────────────

public class CreateReturnOrderHandler : IRequestHandler<CreateReturnOrderCommand, CommandResult>
{
    private readonly IRepository<ReturnOrder> _repo;
    private readonly IOrderRepository _orderRepo;
    private readonly IInventoryService _inventoryService;

    public CreateReturnOrderHandler(
        IRepository<ReturnOrder> repo,
        IOrderRepository orderRepo,
        IInventoryService inventoryService)
    {
        _repo = repo;
        _orderRepo = orderRepo;
        _inventoryService = inventoryService;
    }

    public async Task<CommandResult> Handle(CreateReturnOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepo.GetByIdAsync(request.OrderId);
            if (order is null) return CommandResult.FailureResult("Order not found.");

            // ✅ Build domain ReturnItem child entities
            var returnItems = request.Items
                .Select(i => ReturnItem.Create(i.ProductId, i.Quantity, i.RefundPrice, request.Currency))
                .ToList();

            // ✅ Domain factory validates the return order
            var returnOrder = ReturnOrder.Create(request.OrderId, returnItems, request.Currency);

            // Return stock for each item
            foreach (var item in request.Items)
            {
                await _inventoryService.ReturnStockAsync(
                    item.ProductId, request.WarehouseId,
                    item.Quantity, request.OrderId);
            }

            // ✅ Mark original order as returned via domain method
            order.MarkAsReturned();
            await _orderRepo.UpdateAsync(order);

            await _repo.AddAsync(returnOrder);
            return CommandResult.SuccessResult(returnOrder.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}

public class DeleteReturnOrderHandler : IRequestHandler<DeleteReturnOrderCommand, CommandResult>
{
    private readonly IRepository<ReturnOrder> _repo;
    public DeleteReturnOrderHandler(IRepository<ReturnOrder> repo) => _repo = repo;
    public async Task<CommandResult> Handle(DeleteReturnOrderCommand r, CancellationToken ct)
    {
        var ro = await _repo.GetByIdAsync(r.Id);
        if (ro is null) return CommandResult.FailureResult($"ReturnOrder '{r.Id}' not found.");
        await _repo.DeleteAsync(r.Id);
        return CommandResult.SuccessResult(r.Id);
    }
}

public class GetAllReturnOrdersHandler : IRequestHandler<GetAllReturnOrdersQuery, IEnumerable<ReturnOrderReadDto>>
{
    private readonly IRepository<ReturnOrder> _repo;
    private readonly IMapper _mapper;
    public GetAllReturnOrdersHandler(IRepository<ReturnOrder> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<IEnumerable<ReturnOrderReadDto>> Handle(GetAllReturnOrdersQuery r, CancellationToken ct)
        => _mapper.Map<IEnumerable<ReturnOrderReadDto>>(await _repo.GetAllAsync());
}

public class GetReturnOrderByIdHandler : IRequestHandler<GetReturnOrderByIdQuery, ReturnOrderReadDto?>
{
    private readonly IRepository<ReturnOrder> _repo;
    private readonly IMapper _mapper;
    public GetReturnOrderByIdHandler(IRepository<ReturnOrder> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }
    public async Task<ReturnOrderReadDto?> Handle(GetReturnOrderByIdQuery r, CancellationToken ct)
    {
        var ro = await _repo.GetByIdAsync(r.Id);
        return ro is null ? null : _mapper.Map<ReturnOrderReadDto>(ro);
    }
}
