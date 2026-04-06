namespace BizI.Application.DTOs.Payment;

public record PaymentDto(
    Guid Id,
    Guid OrderId,
    decimal Amount,
    string Currency,
    string Method,
    DateTime CreatedAt);

public record CreatePaymentDto(
    Guid OrderId,
    decimal Amount,
    string Method,
    string Currency = "VND");

public record DebtDto(
    Guid Id,
    Guid CustomerId,
    Guid OrderId,
    decimal Amount,
    decimal PaidAmount,
    decimal RemainingAmount,
    string Status,
    string Currency,
    DateTime CreatedAt);

public record CreateDebtDto(
    Guid CustomerId,
    Guid OrderId,
    decimal Amount,
    string Currency = "VND");

public record RecordDebtPaymentDto(
    Guid DebtId,
    decimal PaidAmount,
    string Currency = "VND");

public record ReturnOrderReadDto(
    Guid Id,
    Guid OrderId,
    decimal TotalRefund,
    string Currency,
    DateTime CreatedAt,
    IReadOnlyList<ReturnItemReadDto> Items);

public record ReturnItemReadDto(
    Guid ProductId,
    int Quantity,
    decimal RefundPrice,
    string Currency);

public record CreateReturnOrderDto(
    Guid OrderId,
    Guid WarehouseId,
    List<CreateReturnItemDto> Items,
    string Currency = "VND");

public record CreateReturnItemDto(
    Guid ProductId,
    int Quantity,
    decimal RefundPrice);
