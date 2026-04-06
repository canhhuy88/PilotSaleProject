namespace BizI.Application.Features.Warehouses.Delete;

public record DeleteWarehouseCommand(Guid Id) : IRequest<CommandResult>;
