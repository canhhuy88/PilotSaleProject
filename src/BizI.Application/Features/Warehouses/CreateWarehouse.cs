namespace BizI.Application.Features.Warehouses;

public record CreateWarehouseCommand(string Name) : IRequest<CommandResult>;

public class CreateWarehouseHandler : IRequestHandler<CreateWarehouseCommand, CommandResult>
{
    private readonly IRepository<Warehouse> _warehouseRepo;

    public CreateWarehouseHandler(IRepository<Warehouse> warehouseRepo)
    {
        _warehouseRepo = warehouseRepo;
    }

    public async Task<CommandResult> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        var warehouse = new Warehouse { Name = request.Name };
        await _warehouseRepo.AddAsync(warehouse);
        return CommandResult.SuccessResult(warehouse.Id);
    }
}
