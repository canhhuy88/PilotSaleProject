using System;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Domain.Enums;
using BizI.Application.Common;

namespace BizI.Application.Features.InventoryTransactions;

public record CreateInventoryTransactionCommand(
    Guid ProductId,
    Guid WarehouseId,
    InventoryTransactionType Type,
    int Quantity,
    Guid? ReferenceId
) : IRequest<CommandResult>;

public class CreateInventoryTransactionCommandValidator : AbstractValidator<CreateInventoryTransactionCommand>
{
    public CreateInventoryTransactionCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Quantity).NotEqual(0);
    }
}

public class CreateInventoryTransactionHandler : IRequestHandler<CreateInventoryTransactionCommand, CommandResult>
{
    private readonly IRepository<InventoryTransaction> _repo;
    private readonly ILogger<CreateInventoryTransactionHandler> _logger;

    public CreateInventoryTransactionHandler(IRepository<InventoryTransaction> repo, ILogger<CreateInventoryTransactionHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateInventoryTransactionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating InventoryTransaction for Product {ProductId}", request.ProductId);

        try
        {
            var entity = new InventoryTransaction
            {
                ProductId = request.ProductId,
                WarehouseId = request.WarehouseId,
                Type = request.Type,
                Quantity = request.Quantity,
                ReferenceId = request.ReferenceId
            };

            await _repo.AddAsync(entity);
            return CommandResult.SuccessResult(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create InventoryTransaction");
            throw;
        }
    }
}
