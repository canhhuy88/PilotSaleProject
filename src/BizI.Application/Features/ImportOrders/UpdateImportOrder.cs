using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.ImportOrders;

public record UpdateImportOrderCommand(
    string Id,
    string SupplierId,
    decimal TotalAmount,
    string Status,
    List<ImportOrderItem> Items
) : IRequest<CommandResult>;

public class UpdateImportOrderCommandValidator : AbstractValidator<UpdateImportOrderCommand>
{
    public UpdateImportOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
    }
}

public class UpdateImportOrderHandler : IRequestHandler<UpdateImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly ILogger<UpdateImportOrderHandler> _logger;

    public UpdateImportOrderHandler(IRepository<ImportOrder> repo, ILogger<UpdateImportOrderHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateImportOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating ImportOrder {Id}", request.Id);

        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.SupplierId = request.SupplierId;
        entity.TotalAmount = request.TotalAmount;
        entity.Status = request.Status;
        entity.Items = request.Items;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
