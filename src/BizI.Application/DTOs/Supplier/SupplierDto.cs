namespace BizI.Application.DTOs.Supplier;

public record SupplierDto(
    string Id,
    string Name,
    string? Phone,
    string? Address);

public record CreateSupplierDto(
    string Name,
    string? Phone = null,
    string? Address = null);

public record UpdateSupplierDto(
    string Id,
    string Name,
    string? Phone = null,
    string? Address = null);
