namespace BizI.Application.Features.Suppliers.Delete;

public record DeleteSupplierCommand(Guid Id) : IRequest<CommandResult>;
