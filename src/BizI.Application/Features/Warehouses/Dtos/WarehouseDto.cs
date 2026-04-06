namespace BizI.Application.Features.Warehouses.Dtos;

public record WarehouseDto(
    Guid Id,
    string Name,
    Guid BranchId);

public record CreateWarehouseDto(string Name, Guid BranchId);

public record UpdateWarehouseDto(Guid Id, string Name, Guid BranchId);
