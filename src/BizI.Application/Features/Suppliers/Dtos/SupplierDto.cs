namespace BizI.Application.Features.Suppliers.Dtos;

public record SupplierDto(
    Guid Id,
    string Name,
    string? Phone,
    string? Address);

public record CreateSupplierDto(
    string Name,
    string? Phone = null,
    string? Address = null);

public record UpdateSupplierDto(
    Guid Id,
    string Name,
    string? Phone = null,
    string? Address = null);
