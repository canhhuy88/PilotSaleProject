namespace BizI.Domain.Entities;

public class CustomerGroup : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal DiscountPercent { get; set; }
}

public class CustomerGroupMapping : BaseEntity
{
    public string CustomerId { get; set; } = string.Empty;
    public string GroupId { get; set; } = string.Empty;
}