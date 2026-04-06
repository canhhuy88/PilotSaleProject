using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Customers;

public record UpdateCustomerCommand(
    string Id,
    string Name,
    string Phone,
    string Address,
    CustomerType CustomerType,
    decimal DebtLimit
) : IRequest<CommandResult>;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
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
        _logger.LogInformation("Handling UpdateCustomerCommand for Id: {CustomerId}", request.Id);

        try
        {
            var customer = await _customerRepo.GetByIdAsync(request.Id);
            if (customer == null)
            {
                return CommandResult.FailureResult("Customer not found");
            }

            customer.Name = request.Name;
            customer.Phone = request.Phone;
            customer.Address = request.Address;
            customer.CustomerType = request.CustomerType;
            customer.DebtLimit = request.DebtLimit;

            await _customerRepo.UpdateAsync(customer);

            _logger.LogInformation("Customer updated successfully with Id: {CustomerId}", customer.Id);

            return CommandResult.SuccessResult(customer.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update customer: {CustomerId}", request.Id);
            throw;
        }
    }
}
