using BizI.Application.DTOs.Customer;
using BizI.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Customers;

// ── Commands/Queries ─────────────────────────────────────────────────────────

public record CreateCustomerCommand(
    string Name,
    string? Phone = null,
    string? Address = null,
    CustomerType CustomerType = CustomerType.Regular,
    decimal DebtLimit = 0m) : IRequest<CommandResult>;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string? Phone = null,
    string? Address = null,
    decimal DebtLimit = 0m) : IRequest<CommandResult>;

public record DeleteCustomerCommand(Guid Id) : IRequest<CommandResult>;

public record GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;

// ── Validators ───────────────────────────────────────────────────────────────

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DebtLimit).GreaterThanOrEqualTo(0);
    }
}

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DebtLimit).GreaterThanOrEqualTo(0);
    }
}

// ── Handlers ─────────────────────────────────────────────────────────────────

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CommandResult>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly ILogger<CreateCustomerHandler> _logger;

    public CreateCustomerHandler(IRepository<Customer> customerRepo, ILogger<CreateCustomerHandler> logger)
    {
        _customerRepo = customerRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating customer: {Name}", request.Name);

        try
        {
            // ✅ Use Domain factory — private setters respected
            var customer = Customer.Create(
                request.Name, request.Phone, request.Address, request.CustomerType);

            if (request.DebtLimit > 0)
                customer.SetDebtLimit(request.DebtLimit);

            await _customerRepo.AddAsync(customer);
            _logger.LogInformation("Customer created. Id: {CustomerId}", customer.Id);
            return CommandResult.SuccessResult(customer.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated creating customer: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CommandResult>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly ILogger<UpdateCustomerHandler> _logger;

    public UpdateCustomerHandler(IRepository<Customer> customerRepo, ILogger<UpdateCustomerHandler> logger)
    {
        _customerRepo = customerRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating customer. Id: {CustomerId}", request.Id);

        var customer = await _customerRepo.GetByIdAsync(request.Id);
        if (customer is null)
            return CommandResult.FailureResult("Customer not found.");

        try
        {
            // ✅ Call domain methods instead of setting properties directly
            customer.Rename(request.Name);

            if (!string.IsNullOrWhiteSpace(request.Phone))
                customer.ChangePhone(request.Phone);

            if (!string.IsNullOrWhiteSpace(request.Address))
                customer.ChangeAddress(request.Address);

            customer.SetDebtLimit(request.DebtLimit);

            await _customerRepo.UpdateAsync(customer);
            _logger.LogInformation("Customer updated. Id: {CustomerId}", customer.Id);
            return CommandResult.SuccessResult(customer.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated updating customer: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}

public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, CommandResult>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly ILogger<DeleteCustomerHandler> _logger;

    public DeleteCustomerHandler(IRepository<Customer> customerRepo, ILogger<DeleteCustomerHandler> logger)
    {
        _customerRepo = customerRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepo.GetByIdAsync(request.Id);
        if (customer is null)
            return CommandResult.FailureResult($"Customer '{request.Id}' not found.");

        await _customerRepo.DeleteAsync(request.Id);
        _logger.LogInformation("Customer deleted. Id: {Id}", request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;

    public GetAllCustomersHandler(IRepository<Customer> customerRepo, IMapper mapper)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepo.GetAllAsync();

        // ✅ Return DTOs — never raw entities
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(IRepository<Customer> customerRepo, IMapper mapper)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepo.GetByIdAsync(request.Id);
        return customer is null ? null : _mapper.Map<CustomerDto>(customer);
    }
}
