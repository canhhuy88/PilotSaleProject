using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;
using System.Collections.Generic;

namespace BizI.Application.Features.ProductVariants;

public record CreateProductVariantCommand(
    string ProductId,
    Dictionary<string, string> Attributes,
    string SKU,
    string? Barcode,
    decimal Price
) : IRequest<CommandResult>;

public class CreateProductVariantCommandValidator : AbstractValidator<CreateProductVariantCommand>
{
    public CreateProductVariantCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.SKU).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}

public class CreateProductVariantHandler : IRequestHandler<CreateProductVariantCommand, CommandResult>
{
    private readonly IRepository<ProductVariant> _repo;
    private readonly ILogger<CreateProductVariantHandler> _logger;

    public CreateProductVariantHandler(IRepository<ProductVariant> repo, ILogger<CreateProductVariantHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateProductVariantCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating ProductVariant {SKU}", request.SKU);
        var entity = new ProductVariant
        {
            ProductId = request.ProductId,
            Attributes = request.Attributes ?? new Dictionary<string, string>(),
            SKU = request.SKU,
            Barcode = request.Barcode,
            Price = request.Price
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
