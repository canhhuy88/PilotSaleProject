namespace BizI.Application.DTOs.Product;

/// <summary>
/// Read DTO returned to API callers — never exposes the Domain entity directly.
/// </summary>
public record ProductDto(
    string Id,
    string Name,
    string SKU,
    string? Description,
    string? Barcode,
    string CategoryId,
    decimal CostPrice,
    decimal SalePrice,
    decimal GrossMarginPercent,
    string Unit,
    bool IsActive);

/// <summary>
/// Input DTO for creating a product.
/// </summary>
public record CreateProductDto(
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    string CategoryId,
    string? Description = null,
    string? Barcode = null);

/// <summary>
/// Input DTO for updating a product.
/// </summary>
public record UpdateProductDto(
    string Id,
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    string CategoryId,
    string? Description = null,
    string? Barcode = null);
