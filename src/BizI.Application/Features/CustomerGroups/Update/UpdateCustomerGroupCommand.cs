namespace BizI.Application.Features.CustomerGroups.Update;

public record UpdateCustomerGroupCommand(Guid Id, string Name, decimal DiscountPercent = 0m) : IRequest<CommandResult>;
