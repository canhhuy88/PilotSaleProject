namespace BizI.Application.Features.StockOperations.Create;

public record CreateStockOperationCommand(
    string Code,
    Guid WarehouseId,
    string OperationType,
    string ReferenceId
) : IRequest<CommandResult>;

public class CreateStockOperationValidator : AbstractValidator<CreateStockOperationCommand>
{
    public CreateStockOperationValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.OperationType).NotEmpty();
    }
}

public class CreateStockOperationHandler : IRequestHandler<CreateStockOperationCommand, CommandResult>
{
    public Task<CommandResult> Handle(CreateStockOperationCommand request, CancellationToken cancellationToken)
        => Task.FromResult(CommandResult.SuccessResult());
}
