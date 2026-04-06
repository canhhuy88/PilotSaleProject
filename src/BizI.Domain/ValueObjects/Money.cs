using BizI.Domain.Exceptions;

namespace BizI.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing a monetary amount with currency.
/// Two Money instances are equal if both Amount and Currency match.
/// </summary>
public sealed class Money : IEquatable<Money>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public static readonly Money Zero = new(0m, "VND");

    private Money() { Amount = 0; Currency = "VND"; } // ORM

    public Money(decimal amount, string currency = "VND")
    {
        if (amount < 0)
            throw new DomainException($"Money amount cannot be negative. Got: {amount}");

        if (string.IsNullOrWhiteSpace(currency))
            throw new DomainException("Currency cannot be empty.");

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        if (Amount < other.Amount)
            throw new DomainException($"Cannot subtract {other.Amount} from {Amount}: result would be negative.");
        return new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal factor)
    {
        if (factor < 0)
            throw new DomainException("Multiplication factor cannot be negative.");
        return new Money(Math.Round(Amount * factor, 2), Currency);
    }

    public bool IsGreaterThan(Money other)
    {
        EnsureSameCurrency(other);
        return Amount > other.Amount;
    }

    public bool IsZero => Amount == 0;

    private void EnsureSameCurrency(Money other)
    {
        if (!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
            throw new DomainException($"Currency mismatch: {Currency} vs {other.Currency}.");
    }

    public bool Equals(Money? other) =>
        other is not null && Amount == other.Amount &&
        string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => Equals(obj as Money);
    public override int GetHashCode() => HashCode.Combine(Amount, Currency.ToUpperInvariant());
    public override string ToString() => $"{Amount:N0} {Currency}";

    public static bool operator ==(Money? left, Money? right) => left?.Equals(right) ?? right is null;
    public static bool operator !=(Money? left, Money? right) => !(left == right);
    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
}
