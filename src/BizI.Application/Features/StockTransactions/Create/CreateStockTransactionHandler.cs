using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.StockTransactions.Create;

public class CreateStockTransactionHandler : IRequestHandler<CreateStockTransactionCommand, CommandResult>
{
    private readonly IRepository<StockTransaction> _repository;
    private readonly ILogger<CreateStockTransactionHandler> _logger;

    public CreateStockTransactionHandler(IRepository<StockTransaction> repository, ILogger<CreateStockTransactionHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateStockTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var transaction = StockTransaction.Create(
                request.Type,
                request.ProductId,
                request.WarehouseId,
                request.Quantity,
                request.BeforeQty,
                request.AfterQty,
                request.RefId,
                request.CreatedBy
            );

            await _repository.AddAsync(transaction);

            return CommandResult.SuccessResult(transaction.Id.ToString());
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated while creating StockTransaction: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}
