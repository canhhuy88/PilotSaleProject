namespace BizI.Application.Features.Suppliers.Update;

public record UpdateSupplierCommand(Guid Id, string Name, string? Phone = null, string? Address = null) : IRequest<CommandResult>;
