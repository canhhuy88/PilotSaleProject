namespace BizI.Domain.Exceptions;

/// <summary>
/// Thrown when an attempt is made to deduct more stock than is available.
/// Accepts both string and Guid product identifiers to support legacy callers.
/// </summary>
public class InsufficientStockException : DomainException
{
    public InsufficientStockException(string productId)
        : base($"Insufficient stock for product '{productId}'.") { }

    public InsufficientStockException(string productId, int available, int requested)
        : base($"Insufficient stock for product '{productId}'. Available: {available}, Requested: {requested}.") { }

    // Backward-compatible overloads for callers that still use Guid
    public InsufficientStockException(Guid productId)
        : this(productId.ToString()) { }

    public InsufficientStockException(Guid productId, int available, int requested)
        : this(productId.ToString(), available, requested) { }
}
