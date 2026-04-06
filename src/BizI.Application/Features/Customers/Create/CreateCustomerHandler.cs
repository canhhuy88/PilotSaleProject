using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Customers.Create;

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
