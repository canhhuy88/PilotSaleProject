namespace BizI.Application.Features.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string SKU,
    string? Description,
    string? Barcode,
    Guid CategoryId,
    decimal CostPrice,
    decimal SalePrice,
    decimal GrossMarginPercent,
    string Unit,
    bool IsActive);

public record CreateProductDto(
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    Guid CategoryId,
    string? Description = null,
    string? Barcode = null);

public record UpdateProductDto(
    Guid Id,
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    Guid CategoryId,
    string? Description = null,
    string? Barcode = null);
