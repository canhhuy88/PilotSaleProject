namespace BizI.Application.DTOs.Payment;

public record PaymentDto(
    string Id,
    string OrderId,
    decimal Amount,
    string Currency,
    string Method,
    DateTime CreatedAt);

public record CreatePaymentDto(
    string OrderId,
    decimal Amount,
    string Method,
    string Currency = "VND");

public record DebtDto(
    string Id,
    string CustomerId,
    string OrderId,
    decimal Amount,
    decimal PaidAmount,
    decimal RemainingAmount,
    string Status,
    string Currency,
    DateTime CreatedAt);

public record CreateDebtDto(
    string CustomerId,
    string OrderId,
    decimal Amount,
    string Currency = "VND");

public record RecordDebtPaymentDto(
    string DebtId,
    decimal PaidAmount,
    string Currency = "VND");

public record ReturnOrderReadDto(
    string Id,
    string OrderId,
    decimal TotalRefund,
    string Currency,
    DateTime CreatedAt,
    IReadOnlyList<ReturnItemReadDto> Items);

public record ReturnItemReadDto(
    string ProductId,
    int Quantity,
    decimal RefundPrice,
    string Currency);

public record CreateReturnOrderDto(
    string OrderId,
    string WarehouseId,
    List<CreateReturnItemDto> Items,
    string Currency = "VND");

public record CreateReturnItemDto(
    string ProductId,
    int Quantity,
    decimal RefundPrice);
