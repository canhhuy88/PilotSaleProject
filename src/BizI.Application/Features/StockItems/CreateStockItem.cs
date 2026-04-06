using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.StockItems;

public record CreateStockItemCommand(
    string ProductId,
    string WarehouseId,
    int Quantity,
    int ReservedQty
) : IRequest<CommandResult>;

public class CreateStockItemCommandValidator : AbstractValidator<CreateStockItemCommand>
{
    public CreateStockItemCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0);
    }
}

public class CreateStockItemHandler : IRequestHandler<CreateStockItemCommand, CommandResult>
{
    private readonly IRepository<StockItem> _repo;
    private readonly ILogger<CreateStockItemHandler> _logger;

    public CreateStockItemHandler(IRepository<StockItem> repo, ILogger<CreateStockItemHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateStockItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating StockItem {ProductId}", request.ProductId);
        var entity = new StockItem
        {
            ProductId = request.ProductId,
            WarehouseId = request.WarehouseId,
            Quantity = request.Quantity,
            ReservedQty = request.ReservedQty
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
