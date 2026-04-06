using System;

namespace BizI.Domain.Exceptions;

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(Guid productId) : base($"Insufficient stock for product {productId}") { }
}
