using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.StockItems;

public record UpdateStockItemCommand(
    string Id,
    string ProductId,
    string WarehouseId,
    int Quantity,
    int ReservedQty
) : IRequest<CommandResult>;

public class UpdateStockItemCommandValidator : AbstractValidator<UpdateStockItemCommand>
{
    public UpdateStockItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class UpdateStockItemHandler : IRequestHandler<UpdateStockItemCommand, CommandResult>
{
    private readonly IRepository<StockItem> _repo;
    private readonly ILogger<UpdateStockItemHandler> _logger;

    public UpdateStockItemHandler(IRepository<StockItem> repo, ILogger<UpdateStockItemHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateStockItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating StockItem {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.ProductId = request.ProductId;
        entity.WarehouseId = request.WarehouseId;
        entity.Quantity = request.Quantity;
        entity.ReservedQty = request.ReservedQty;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
