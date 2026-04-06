using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.StockOperations;

public record CreateStockOutCommand(
    string Code,
    string WarehouseId,
    string Reason,
    List<StockOutItem> Items
) : IRequest<CommandResult>;

public class CreateStockOutHandler : IRequestHandler<CreateStockOutCommand, CommandResult>
{
    private readonly IRepository<StockOut> _repo;

    public CreateStockOutHandler(IRepository<StockOut> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateStockOutCommand request, CancellationToken cancellationToken)
    {
        var entity = new StockOut
        {
            Code = request.Code,
            WarehouseId = request.WarehouseId,
            Reason = request.Reason,
            Items = request.Items ?? new List<StockOutItem>()
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
