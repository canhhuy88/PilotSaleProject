using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.StockOperations;

public record CreateStockInCommand(
    string Code,
    string SupplierId,
    string WarehouseId,
    decimal TotalAmount,
    List<StockInItem> Items
) : IRequest<CommandResult>;

public class CreateStockInHandler : IRequestHandler<CreateStockInCommand, CommandResult>
{
    private readonly IRepository<StockIn> _repo;

    public CreateStockInHandler(IRepository<StockIn> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateStockInCommand request, CancellationToken cancellationToken)
    {
        var entity = new StockIn
        {
            Code = request.Code,
            SupplierId = request.SupplierId,
            WarehouseId = request.WarehouseId,
            TotalAmount = request.TotalAmount,
            Items = request.Items ?? new List<StockInItem>()
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
