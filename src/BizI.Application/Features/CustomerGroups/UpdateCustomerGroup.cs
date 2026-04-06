using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.CustomerGroups;

public record UpdateCustomerGroupCommand(
    string Id,
    string Name,
    decimal DiscountPercent
) : IRequest<CommandResult>;

public class UpdateCustomerGroupCommandValidator : AbstractValidator<UpdateCustomerGroupCommand>
{
    public UpdateCustomerGroupCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
    }
}

public class UpdateCustomerGroupHandler : IRequestHandler<UpdateCustomerGroupCommand, CommandResult>
{
    private readonly IRepository<CustomerGroup> _customerGroupRepo;
    private readonly ILogger<UpdateCustomerGroupHandler> _logger;

    public UpdateCustomerGroupHandler(IRepository<CustomerGroup> customerGroupRepo, ILogger<UpdateCustomerGroupHandler> logger)
    {
        _customerGroupRepo = customerGroupRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateCustomerGroupCommand for Id: {CustomerGroupId}", request.Id);

        try
        {
            var customerGroup = await _customerGroupRepo.GetByIdAsync(request.Id);
            if (customerGroup == null)
            {
                return CommandResult.FailureResult("CustomerGroup not found");
            }

            customerGroup.Name = request.Name;
            customerGroup.DiscountPercent = request.DiscountPercent;

            await _customerGroupRepo.UpdateAsync(customerGroup);

            _logger.LogInformation("CustomerGroup updated successfully with Id: {CustomerGroupId}", customerGroup.Id);

            return CommandResult.SuccessResult(customerGroup.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update customer group: {CustomerGroupId}", request.Id);
            throw;
        }
    }
}
