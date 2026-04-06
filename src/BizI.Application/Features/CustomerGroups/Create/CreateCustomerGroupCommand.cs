namespace BizI.Application.Features.CustomerGroups.Create;

public record CreateCustomerGroupCommand(string Name, decimal DiscountPercent = 0m) : IRequest<CommandResult>;
