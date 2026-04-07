namespace BizI.Application.Features.StockItems.Create;

public record CreateStockItemCommand(
    Guid ProductId,
    Guid WarehouseId,
    int Quantity
) : IRequest<CommandResult>;

public class CreateStockItemValidator : AbstractValidator<CreateStockItemCommand>
{
    public CreateStockItemValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}

public class CreateStockItemHandler : IRequestHandler<CreateStockItemCommand, CommandResult>
{
    public Task<CommandResult> Handle(CreateStockItemCommand request, CancellationToken cancellationToken)
        => Task.FromResult(CommandResult.SuccessResult());
}
