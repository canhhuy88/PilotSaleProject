using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.StockTransactions;

public record UpdateStockTransactionCommand(
    string Id,
    StockTransactionType Type,
    string RefId,
    string ProductId,
    string WarehouseId,
    int Quantity,
    int BeforeQty,
    int AfterQty,
    string CreatedBy
) : IRequest<CommandResult>;

public class UpdateStockTransactionCommandValidator : AbstractValidator<UpdateStockTransactionCommand>
{
    public UpdateStockTransactionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class UpdateStockTransactionHandler : IRequestHandler<UpdateStockTransactionCommand, CommandResult>
{
    private readonly IRepository<StockTransaction> _repo;
    private readonly ILogger<UpdateStockTransactionHandler> _logger;

    public UpdateStockTransactionHandler(IRepository<StockTransaction> repo, ILogger<UpdateStockTransactionHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateStockTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating StockTransaction {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.Type = request.Type;
        entity.RefId = request.RefId;
        entity.ProductId = request.ProductId;
        entity.WarehouseId = request.WarehouseId;
        entity.Quantity = request.Quantity;
        entity.BeforeQty = request.BeforeQty;
        entity.AfterQty = request.AfterQty;
        entity.CreatedBy = request.CreatedBy;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
