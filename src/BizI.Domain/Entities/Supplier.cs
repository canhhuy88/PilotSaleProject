using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a goods supplier.
/// </summary>
public class Supplier : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    /// <summary>Validated contact phone number.</summary>
    public PhoneNumber? Phone { get; private set; }

    /// <summary>Physical or correspondence address.</summary>
    public Address? ContactAddress { get; private set; }

    private Supplier() { } // ORM / serialization

    /// <summary>Factory — creates a valid Supplier.</summary>
    public static Supplier Create(string name, string? phone = null, string? address = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Supplier name cannot be empty.");

        return new Supplier
        {
            Name = name.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : new PhoneNumber(phone),
            ContactAddress = string.IsNullOrWhiteSpace(address) ? null : Address.FromString(address)
        };
    }

    /// <summary>Renames the supplier.</summary>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Supplier name cannot be empty.");

        Name = newName.Trim();
        Touch();
    }

    /// <summary>Updates the supplier's contact phone number.</summary>
    public void ChangePhone(string phone)
    {
        Phone = new PhoneNumber(phone);
        Touch();
    }

    /// <summary>Updates the supplier's address.</summary>
    public void ChangeAddress(string address)
    {
        ContactAddress = Address.FromString(address);
        Touch();
    }
}
