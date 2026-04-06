using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.StockOperations;

public record CreateStockTransferCommand(
    string FromWarehouseId,
    string ToWarehouseId,
    List<StockTransferItem> Items
) : IRequest<CommandResult>;

public class CreateStockTransferHandler : IRequestHandler<CreateStockTransferCommand, CommandResult>
{
    private readonly IRepository<StockTransfer> _repo;

    public CreateStockTransferHandler(IRepository<StockTransfer> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateStockTransferCommand request, CancellationToken cancellationToken)
    {
        var entity = new StockTransfer
        {
            FromWarehouseId = request.FromWarehouseId,
            ToWarehouseId = request.ToWarehouseId,
            Items = request.Items ?? new List<StockTransferItem>()
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
