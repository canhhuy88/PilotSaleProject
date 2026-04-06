using System;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Domain.Enums;
using BizI.Application.Common;

namespace BizI.Application.Features.InventoryTransactions;

public record UpdateInventoryTransactionCommand(
    string Id,
    Guid ProductId,
    Guid WarehouseId,
    InventoryTransactionType Type,
    int Quantity,
    Guid? ReferenceId
) : IRequest<CommandResult>;

public class UpdateInventoryTransactionCommandValidator : AbstractValidator<UpdateInventoryTransactionCommand>
{
    public UpdateInventoryTransactionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class UpdateInventoryTransactionHandler : IRequestHandler<UpdateInventoryTransactionCommand, CommandResult>
{
    private readonly IRepository<InventoryTransaction> _repo;
    private readonly ILogger<UpdateInventoryTransactionHandler> _logger;

    public UpdateInventoryTransactionHandler(IRepository<InventoryTransaction> repo, ILogger<UpdateInventoryTransactionHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating InventoryTransaction {Id}", request.Id);

        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.ProductId = request.ProductId;
        entity.WarehouseId = request.WarehouseId;
        entity.Type = request.Type;
        entity.Quantity = request.Quantity;
        entity.ReferenceId = request.ReferenceId;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
