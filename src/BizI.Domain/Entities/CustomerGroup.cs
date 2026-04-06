using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents a named pricing group that provides a shared discount to all members.
/// </summary>
public class CustomerGroup : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    /// <summary>Discount percentage applied to all members of this group (0–100).</summary>
    public decimal DiscountPercent { get; private set; }

    private CustomerGroup() { } // ORM / serialization

    /// <summary>Factory — creates a valid CustomerGroup.</summary>
    public static CustomerGroup Create(string name, decimal discountPercent = 0m)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer group name cannot be empty.");

        ValidateDiscount(discountPercent);

        return new CustomerGroup
        {
            Name = name.Trim(),
            DiscountPercent = discountPercent
        };
    }

    /// <summary>Renames the group.</summary>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Customer group name cannot be empty.");

        Name = newName.Trim();
        Touch();
    }

    /// <summary>Updates the discount percentage.</summary>
    public void SetDiscount(decimal discountPercent)
    {
        ValidateDiscount(discountPercent);
        DiscountPercent = discountPercent;
        Touch();
    }

    private static void ValidateDiscount(decimal discount)
    {
        if (discount < 0 || discount > 100)
            throw new DomainException("Discount percentage must be between 0 and 100.");
    }
}

/// <summary>
/// Join table mapping a Customer to a CustomerGroup.
/// Kept as a separate entity so it can be audited and time-stamped.
/// </summary>
public class CustomerGroupMapping : BaseEntity
{
    public string CustomerId { get; private set; } = string.Empty;
    public string GroupId { get; private set; } = string.Empty;

    private CustomerGroupMapping() { } // ORM

    public static CustomerGroupMapping Create(string customerId, string groupId)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new DomainException("CustomerId cannot be empty.");

        if (string.IsNullOrWhiteSpace(groupId))
            throw new DomainException("GroupId cannot be empty.");

        return new CustomerGroupMapping
        {
            CustomerId = customerId.Trim(),
            GroupId = groupId.Trim()
        };
    }
}
