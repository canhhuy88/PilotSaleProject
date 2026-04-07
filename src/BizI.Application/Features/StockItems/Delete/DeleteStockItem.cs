namespace BizI.Application.Features.StockItems.Delete;

public record DeleteStockItemCommand(Guid Id) : IRequest<CommandResult>;

public class DeleteStockItemHandler : IRequestHandler<DeleteStockItemCommand, CommandResult>
{
    public Task<CommandResult> Handle(DeleteStockItemCommand request, CancellationToken cancellationToken)
        => Task.FromResult(CommandResult.SuccessResult());
}
