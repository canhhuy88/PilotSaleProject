namespace BizI.Application.Features.Suppliers.Create;

public record CreateSupplierCommand(string Name, string? Phone = null, string? Address = null) : IRequest<CommandResult>;
