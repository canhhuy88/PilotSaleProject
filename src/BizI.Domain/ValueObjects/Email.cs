using System.Text.RegularExpressions;
using BizI.Domain.Exceptions;

namespace BizI.Domain.ValueObjects;

/// <summary>
/// Immutable value object representing a validated e-mail address.
/// </summary>
public sealed class Email : IEquatable<Email>
{
    private static readonly Regex EmailRegex =
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    private Email() { Value = string.Empty; } // ORM

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Email address cannot be empty.");

        if (!EmailRegex.IsMatch(value))
            throw new DomainException($"'{value}' is not a valid email address.");

        Value = value.Trim().ToLowerInvariant();
    }

    public static Email? TryCreate(string? value) =>
        string.IsNullOrWhiteSpace(value) || !EmailRegex.IsMatch(value)
            ? null
            : new Email(value);

    public bool Equals(Email? other) =>
        other is not null && string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => Equals(obj as Email);
    public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}
