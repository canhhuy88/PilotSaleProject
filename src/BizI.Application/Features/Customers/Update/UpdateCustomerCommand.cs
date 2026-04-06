namespace BizI.Application.Features.Customers.Update;

public record UpdateCustomerCommand(
    Guid Id,
    string Name,
    string? Phone = null,
    string? Address = null,
    decimal DebtLimit = 0m) : IRequest<CommandResult>;
