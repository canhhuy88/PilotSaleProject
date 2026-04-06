namespace BizI.Application.Features.CustomerGroups.Delete;

public record DeleteCustomerGroupCommand(Guid Id) : IRequest<CommandResult>;
