namespace BizI.Application.Features.Warehouses.Update;

public record UpdateWarehouseCommand(Guid Id, string Name, Guid BranchId) : IRequest<CommandResult>;
