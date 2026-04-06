using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Customers.Update;

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
