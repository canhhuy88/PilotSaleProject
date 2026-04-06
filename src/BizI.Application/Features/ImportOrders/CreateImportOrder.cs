using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.ImportOrders;

public record CreateImportOrderCommand(
    string SupplierId,
    decimal TotalAmount,
    string Status,
    List<ImportOrderItem> Items
) : IRequest<CommandResult>;

public class CreateImportOrderCommandValidator : AbstractValidator<CreateImportOrderCommand>
{
    public CreateImportOrderCommandValidator()
    {
        RuleFor(x => x.SupplierId).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Status).NotEmpty();
        RuleFor(x => x.Items).NotNull();
    }
}

public class CreateImportOrderHandler : IRequestHandler<CreateImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;
    private readonly ILogger<CreateImportOrderHandler> _logger;

    public CreateImportOrderHandler(IRepository<ImportOrder> repo, ILogger<CreateImportOrderHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateImportOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating ImportOrder for Supplier {SupplierId}", request.SupplierId);

        try
        {
            var entity = new ImportOrder
            {
                SupplierId = request.SupplierId,
                TotalAmount = request.TotalAmount,
                Status = request.Status,
                Items = request.Items ?? new List<ImportOrderItem>()
            };

            await _repo.AddAsync(entity);
            return CommandResult.SuccessResult(entity.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create ImportOrder");
            throw;
        }
    }
}
