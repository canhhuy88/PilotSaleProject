using BizI.Domain.Exceptions;

namespace BizI.Domain.ValueObjects;

/// <summary>
/// Immutable value object for a physical or shipping address.
/// </summary>
public sealed class Address : IEquatable<Address>
{
    public string Street { get; }
    public string? District { get; }
    public string? City { get; }
    public string? Province { get; }
    public string? Country { get; }

    private Address()
    {
        Street = string.Empty;
    } // ORM

    public Address(
        string street,
        string? district = null,
        string? city = null,
        string? province = null,
        string? country = "Vietnam")
    {
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be empty.");

        Street = street.Trim();
        District = district?.Trim();
        City = city?.Trim();
        Province = province?.Trim();
        Country = country?.Trim();
    }

    /// <summary>Creates an Address from a plain string (legacy single-line address).</summary>
    public static Address FromString(string rawAddress)
    {
        if (string.IsNullOrWhiteSpace(rawAddress))
            throw new DomainException("Address cannot be empty.");

        return new Address(rawAddress.Trim());
    }

    public static Address? TryFromString(string? rawAddress) =>
        string.IsNullOrWhiteSpace(rawAddress) ? null : new Address(rawAddress.Trim());

    public string FullAddress =>
        string.Join(", ", new[] { Street, District, City, Province, Country }
            .Where(s => !string.IsNullOrWhiteSpace(s)));

    public bool Equals(Address? other) =>
        other is not null &&
        string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(District, other.District, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(Province, other.Province, StringComparison.OrdinalIgnoreCase) &&
        string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode() =>
        HashCode.Combine(
            Street?.ToLowerInvariant(),
            District?.ToLowerInvariant(),
            City?.ToLowerInvariant(),
            Province?.ToLowerInvariant(),
            Country?.ToLowerInvariant());

    public override string ToString() => FullAddress;
}
