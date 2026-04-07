namespace BizI.Application.Features.StockItems.Update;

public record UpdateStockItemCommand(
    Guid Id,
    int Quantity,
    int ReservedQty
) : IRequest<CommandResult>;

public class UpdateStockItemValidator : AbstractValidator<UpdateStockItemCommand>
{
    public UpdateStockItemValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ReservedQty).GreaterThanOrEqualTo(0);
    }
}

public class UpdateStockItemHandler : IRequestHandler<UpdateStockItemCommand, CommandResult>
{
    public Task<CommandResult> Handle(UpdateStockItemCommand request, CancellationToken cancellationToken)
        => Task.FromResult(CommandResult.SuccessResult());
}
