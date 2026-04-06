using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.CustomerGroups;

public record CreateCustomerGroupCommand(
    string Name,
    decimal DiscountPercent
) : IRequest<CommandResult>;

public class CreateCustomerGroupCommandValidator : AbstractValidator<CreateCustomerGroupCommand>
{
    public CreateCustomerGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
    }
}

public class CreateCustomerGroupHandler : IRequestHandler<CreateCustomerGroupCommand, CommandResult>
{
    private readonly IRepository<CustomerGroup> _customerGroupRepo;
    private readonly ILogger<CreateCustomerGroupHandler> _logger;

    public CreateCustomerGroupHandler(IRepository<CustomerGroup> customerGroupRepo, ILogger<CreateCustomerGroupHandler> logger)
    {
        _customerGroupRepo = customerGroupRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateCustomerGroupCommand: {CustomerGroupName}", request.Name);

        try
        {
            var customerGroup = new CustomerGroup
            {
                Name = request.Name,
                DiscountPercent = request.DiscountPercent
            };

            await _customerGroupRepo.AddAsync(customerGroup);

            _logger.LogInformation("CustomerGroup created successfully with Id: {CustomerGroupId}", customerGroup.Id);

            return CommandResult.SuccessResult(customerGroup.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create customer group: {CustomerGroupName}", request.Name);
            throw;
        }
    }
}
