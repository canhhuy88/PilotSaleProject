namespace BizI.Application.DTOs.Warehouse;

public record WarehouseDto(
    string Id,
    string Name,
    string BranchId);

public record CreateWarehouseDto(string Name, string BranchId);

public record UpdateWarehouseDto(string Id, string Name, string BranchId);
