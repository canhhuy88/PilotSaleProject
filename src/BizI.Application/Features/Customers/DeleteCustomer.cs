using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Customers;

public record DeleteCustomerCommand(string Id) : IRequest<CommandResult>;

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
        _logger.LogInformation("Handling DeleteCustomerCommand for Id: {CustomerId}", request.Id);

        try
        {
            await _customerRepo.DeleteAsync(request.Id);
            _logger.LogInformation("Customer deleted successfully with Id: {CustomerId}", request.Id);

            return CommandResult.SuccessResult(request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete customer: {CustomerId}", request.Id);
            throw;
        }
    }
}
