using System;

namespace BizI.Domain.Exceptions;

public class OverReturnException : DomainException
{
    public OverReturnException(Guid productId, int requested, int remaining)
        : base($"Product {productId}: Cannot return {requested}. Only {remaining} remaining from original sale.") { }
}
