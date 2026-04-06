namespace BizI.Application.Features.Products.Create;

public record CreateProductCommand(
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    Guid CategoryId,
    string? Description = null,
    string? Barcode = null,
    string Currency = "VND") : IRequest<CommandResult>;
