using System;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Inventory;

public record UpdateInventoryCommand(string Id, Guid ProductId, Guid WarehouseId, int Quantity) : IRequest<CommandResult>;

public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
    }
}

public class UpdateInventoryHandler : IRequestHandler<UpdateInventoryCommand, CommandResult>
{
    private readonly IRepository<BizI.Domain.Entities.Inventory> _repo;

    public UpdateInventoryHandler(IRepository<BizI.Domain.Entities.Inventory> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.ProductId = request.ProductId;
        entity.WarehouseId = request.WarehouseId;
        entity.Quantity = request.Quantity;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
