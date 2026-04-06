namespace BizI.Application.DTOs.Product;

/// <summary>
/// Read DTO returned to API callers — never exposes the Domain entity directly.
/// </summary>
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

/// <summary>
/// Input DTO for creating a product.
/// </summary>
public record CreateProductDto(
    string Name,
    string SKU,
    decimal CostPrice,
    decimal SalePrice,
    string Unit,
    Guid CategoryId,
    string? Description = null,
    string? Barcode = null);

/// <summary>
/// Input DTO for updating a product.
/// </summary>
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
