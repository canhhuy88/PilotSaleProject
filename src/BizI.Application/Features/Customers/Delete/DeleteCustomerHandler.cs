using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Customers.Delete;

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
