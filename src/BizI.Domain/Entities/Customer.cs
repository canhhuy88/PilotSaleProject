using System;

namespace BizI.Domain.Entities;

public enum CustomerType { WALKIN, REGULAR, VIP, WHOLESALE }

public class Customer : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public CustomerType CustomerType { get; set; } = CustomerType.REGULAR;
    public int LoyaltyPoint { get; set; }
    public string LoyaltyTier { get; set; } = "Silver";
    public decimal TotalSpent { get; set; }
    public int TotalOrders { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public decimal DebtTotal { get; set; }
    public decimal DebtLimit { get; set; }
}