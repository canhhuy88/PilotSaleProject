using System.Text.RegularExpressions;
using BizI.Domain.Exceptions;

namespace BizI.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing a validated phone number.
/// Accepts local Vietnamese formats and international E.164.
/// </summary>
public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    private static readonly Regex PhoneRegex =
        new(@"^[\+]?[\d\s\-\(\)]{7,20}$", RegexOptions.Compiled);

    public string Value { get; }

    private PhoneNumber() { Value = string.Empty; } // ORM

    public PhoneNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Phone number cannot be empty.");

        var normalized = value.Trim();
        if (!PhoneRegex.IsMatch(normalized))
            throw new DomainException($"'{normalized}' is not a valid phone number.");

        Value = normalized;
    }

    public static PhoneNumber? TryCreate(string? value) =>
        string.IsNullOrWhiteSpace(value) || !PhoneRegex.IsMatch(value.Trim())
            ? null
            : new PhoneNumber(value);

    public bool Equals(PhoneNumber? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.Ordinal);

    public override bool Equals(object? obj) => Equals(obj as PhoneNumber);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(PhoneNumber phone) => phone.Value;
}
