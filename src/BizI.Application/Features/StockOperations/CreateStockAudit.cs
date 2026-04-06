using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.StockOperations;

public record CreateStockAuditCommand(
    string WarehouseId,
    List<StockAuditItem> Items
) : IRequest<CommandResult>;

public class CreateStockAuditHandler : IRequestHandler<CreateStockAuditCommand, CommandResult>
{
    private readonly IRepository<StockAudit> _repo;

    public CreateStockAuditHandler(IRepository<StockAudit> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateStockAuditCommand request, CancellationToken cancellationToken)
    {
        var entity = new StockAudit
        {
            WarehouseId = request.WarehouseId,
            Items = request.Items ?? new List<StockAuditItem>()
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
