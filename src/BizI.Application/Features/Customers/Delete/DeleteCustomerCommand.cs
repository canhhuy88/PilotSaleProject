namespace BizI.Application.Features.Customers.Delete;

public record DeleteCustomerCommand(Guid Id) : IRequest<CommandResult>;
