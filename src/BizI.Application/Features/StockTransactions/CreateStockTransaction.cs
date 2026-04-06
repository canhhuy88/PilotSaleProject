using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.StockTransactions;

public record CreateStockTransactionCommand(
    StockTransactionType Type,
    string RefId,
    string ProductId,
    string WarehouseId,
    int Quantity,
    int BeforeQty,
    int AfterQty,
    string CreatedBy
) : IRequest<CommandResult>;

public class CreateStockTransactionCommandValidator : AbstractValidator<CreateStockTransactionCommand>
{
    public CreateStockTransactionCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Quantity).NotEqual(0);
    }
}

public class CreateStockTransactionHandler : IRequestHandler<CreateStockTransactionCommand, CommandResult>
{
    private readonly IRepository<StockTransaction> _repo;
    private readonly ILogger<CreateStockTransactionHandler> _logger;

    public CreateStockTransactionHandler(IRepository<StockTransaction> repo, ILogger<CreateStockTransactionHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateStockTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating StockTransaction");
        var entity = new StockTransaction
        {
            Type = request.Type,
            RefId = request.RefId,
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            Quantity = request.Quantity,
            BeforeQty = request.BeforeQty,
            AfterQty = request.AfterQty,
            CreatedBy = request.CreatedBy
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
