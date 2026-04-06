namespace BizI.Application.Features.Warehouses.Create;

public record CreateWarehouseCommand(string Name, Guid BranchId) : IRequest<CommandResult>;
