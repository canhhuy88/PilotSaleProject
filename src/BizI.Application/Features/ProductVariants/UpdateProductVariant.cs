using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.ProductVariants;

public record UpdateProductVariantCommand(
    string Id,
    string ProductId,
    Dictionary<string, string> Attributes,
    string SKU,
    string? Barcode,
    decimal Price
) : IRequest<CommandResult>;

public class UpdateProductVariantCommandValidator : AbstractValidator<UpdateProductVariantCommand>
{
    public UpdateProductVariantCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.SKU).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class UpdateProductVariantHandler : IRequestHandler<UpdateProductVariantCommand, CommandResult>
{
    private readonly IRepository<ProductVariant> _repo;
    private readonly ILogger<UpdateProductVariantHandler> _logger;

    public UpdateProductVariantHandler(IRepository<ProductVariant> repo, ILogger<UpdateProductVariantHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateProductVariantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating ProductVariant {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.ProductId = request.ProductId;
        entity.Attributes = request.Attributes;
        entity.SKU = request.SKU;
        entity.Barcode = request.Barcode;
        entity.Price = request.Price;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
