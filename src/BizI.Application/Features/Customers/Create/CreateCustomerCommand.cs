namespace BizI.Application.Features.Customers.Create;

public record CreateCustomerCommand(
    string Name,
    string? Phone = null,
    string? Address = null,
    CustomerType CustomerType = CustomerType.Regular,
    decimal DebtLimit = 0m) : IRequest<CommandResult>;
