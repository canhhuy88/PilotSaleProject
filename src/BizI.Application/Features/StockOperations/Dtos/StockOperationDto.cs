namespace BizI.Application.Features.StockOperations.Dtos;

public record StockOperationDto(
    Guid Id,
    string Code,
    Guid WarehouseId,
    string OperationType,
    string ReferenceId,
    DateTime CreatedAt
);
