using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Customers;

public record CreateCustomerCommand(
    string Name,
    string Phone,
    string Address,
    CustomerType CustomerType,
    decimal DebtLimit
) : IRequest<CommandResult>;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
    }
}

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
        _logger.LogInformation("Handling CreateCustomerCommand: {CustomerName}", request.Name);

        try
        {
            var customer = new Customer
            {
                Name = request.Name,
                Phone = request.Phone,
                Address = request.Address,
                CustomerType = request.CustomerType,
                DebtLimit = request.DebtLimit,
                LoyaltyTier = "Silver"
            };

            await _customerRepo.AddAsync(customer);

            _logger.LogInformation("Customer created successfully with Id: {CustomerId}", customer.Id);

            return CommandResult.SuccessResult(customer.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create customer: {CustomerName}", request.Name);
            throw;
        }
    }
}
