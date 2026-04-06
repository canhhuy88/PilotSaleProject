namespace BizI.Domain.Exceptions;

/// <summary>
/// Thrown when an attempt is made to deduct more stock than is available.
/// Accepts both string and Guid product identifiers to support legacy callers.
/// </summary>
public class InsufficientStockException : DomainException
{
    public InsufficientStockException(Guid productId)
        : base($"Insufficient stock for product '{productId}'.") { }

    public InsufficientStockException(Guid productId, int available, int requested)
        : base($"Insufficient stock for product '{productId}'. Available: {available}, Requested: {requested}.") { }


}

