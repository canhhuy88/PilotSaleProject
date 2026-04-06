using BizI.Domain.Enums;
using BizI.Domain.Exceptions;
using BizI.Domain.ValueObjects;

namespace BizI.Domain.Entities;

/// <summary>
/// Aggregate Root: represents a store customer with loyalty and debt tracking.
/// All state mutation is performed through explicit domain methods.
/// </summary>
public class Customer : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    /// <summary>Contact phone number (ValueObject validates format).</summary>
    public PhoneNumber? Phone { get; private set; }

    /// <summary>Physical / delivery address (ValueObject).</summary>
    public Address? ContactAddress { get; private set; }

    public CustomerType CustomerType { get; private set; } = CustomerType.Regular;
    public int LoyaltyPoints { get; private set; }
    public string LoyaltyTier { get; private set; } = "Silver";

    /// <summary>Cumulative spend across all orders.</summary>
    public decimal TotalSpent { get; private set; }
    public int TotalOrders { get; private set; }
    public DateTime? LastOrderDate { get; private set; }

    /// <summary>Outstanding balance owed by the customer.</summary>
    public decimal DebtTotal { get; private set; }

    /// <summary>Maximum allowable debt (0 = unlimited).</summary>
    public decimal DebtLimit { get; private set; }

    private Customer() { } // ORM / serialization

    // ──────────────────────────────────────────────
    //  Factory Methods
    // ──────────────────────────────────────────────

    /// <summary>Creates a regular / walk-in customer.</summary>
    public static Customer Create(
        string name,
        string? phone = null,
        string? address = null,
        CustomerType customerType = CustomerType.Regular)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer name cannot be empty.");

        return new Customer
        {
            Name = name.Trim(),
            Phone = string.IsNullOrWhiteSpace(phone) ? null : new PhoneNumber(phone),
            ContactAddress = string.IsNullOrWhiteSpace(address) ? null : Address.FromString(address),
            CustomerType = customerType
        };
    }

    /// <summary>Walk-in customer factory (special pre-seeded customer).</summary>
    public static Customer CreateWalkIn() =>
        new Customer
        {
            //Id = "WALKIN",
            Name = "Walk-in Customer",
            CustomerType = CustomerType.WalkIn
        };

    // ──────────────────────────────────────────────
    //  Domain Methods
    // ──────────────────────────────────────────────

    /// <summary>Updates the customer's display name.</summary>
    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new DomainException("Customer name cannot be empty.");

        Name = newName.Trim();
        Touch();
    }

    /// <summary>Replaces the customer's phone number.</summary>
    public void ChangePhone(string phone)
    {
        Phone = new PhoneNumber(phone);
        Touch();
    }

    /// <summary>Replaces the customer's contact address.</summary>
    public void ChangeAddress(string address)
    {
        ContactAddress = Address.FromString(address);
        Touch();
    }

    /// <summary>Awards loyalty points and re-evaluates the tier.</summary>
    public void EarnLoyaltyPoints(int points)
    {
        if (points <= 0) return;
        LoyaltyPoints += points;
        RecalculateTier();
        Touch();
    }

    /// <summary>Redeems loyalty points; throws if insufficient balance.</summary>
    public void RedeemLoyaltyPoints(int points)
    {
        if (points > LoyaltyPoints)
            throw new DomainException($"Insufficient loyalty points. Available: {LoyaltyPoints}, Requested: {points}.");

        LoyaltyPoints -= points;
        Touch();
    }

    /// <summary>Records a completed order against the customer's statistics.</summary>
    public void RecordOrder(decimal orderAmount)
    {
        if (orderAmount < 0)
            throw new DomainException("Order amount cannot be negative.");

        TotalSpent += orderAmount;
        TotalOrders++;
        LastOrderDate = DateTime.UtcNow;
        Touch();
    }

    /// <summary>Increases the customer's outstanding debt balance.</summary>
    public void AddDebt(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Debt amount must be positive.");

        if (DebtLimit > 0 && DebtTotal + amount > DebtLimit)
            throw new DomainException($"Customer debt limit of {DebtLimit:N0} would be exceeded.");

        DebtTotal += amount;
        Touch();
    }

    /// <summary>Reduces the customer's outstanding debt.</summary>
    public void ClearDebt(decimal amount)
    {
        if (amount <= 0)
            throw new DomainException("Payment amount must be positive.");

        DebtTotal = Math.Max(0, DebtTotal - amount);
        Touch();
    }

    /// <summary>Sets the maximum allowable debt (0 = no limit).</summary>
    public void SetDebtLimit(decimal limit)
    {
        if (limit < 0)
            throw new DomainException("Debt limit cannot be negative.");

        DebtLimit = limit;
        Touch();
    }

    // ──────────────────────────────────────────────
    //  Private Helpers
    // ──────────────────────────────────────────────

    private void RecalculateTier()
    {
        LoyaltyTier = LoyaltyPoints switch
        {
            >= 10000 => "Platinum",
            >= 5000 => "Gold",
            >= 1000 => "Silver",
            _ => "Bronze"
        };
    }
}
