using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.CustomerGroups;

public record DeleteCustomerGroupCommand(string Id) : IRequest<CommandResult>;

public class DeleteCustomerGroupHandler : IRequestHandler<DeleteCustomerGroupCommand, CommandResult>
{
    private readonly IRepository<CustomerGroup> _customerGroupRepo;
    private readonly ILogger<DeleteCustomerGroupHandler> _logger;

    public DeleteCustomerGroupHandler(IRepository<CustomerGroup> customerGroupRepo, ILogger<DeleteCustomerGroupHandler> logger)
    {
        _customerGroupRepo = customerGroupRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteCustomerGroupCommand for Id: {CustomerGroupId}", request.Id);

        try
        {
            await _customerGroupRepo.DeleteAsync(request.Id);
            _logger.LogInformation("CustomerGroup deleted successfully with Id: {CustomerGroupId}", request.Id);

            return CommandResult.SuccessResult(request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete customer group: {CustomerGroupId}", request.Id);
            throw;
        }
    }
}
