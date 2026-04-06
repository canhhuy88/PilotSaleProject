using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Products;

public record UpdateProductCommand(
    string Id,
    string Name,
    string SKU,
    string? Description,
    string? Barcode,
    string CategoryId,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    bool IsActive
) : IRequest<CommandResult>;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.SalePrice).GreaterThanOrEqualTo(0);
    }
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, CommandResult>
{
    private readonly IRepository<Product> _repo;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(IRepository<Product> repo, ILogger<UpdateProductHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Product {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.Name = request.Name;
        entity.SKU = request.SKU;
        entity.Description = request.Description;
        entity.Barcode = request.Barcode;
        entity.CategoryId = request.CategoryId;
        entity.CostPrice = request.CostPrice;
        entity.SalePrice = request.SalePrice;
        entity.Unit = request.Unit;
        entity.IsActive = request.IsActive;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
