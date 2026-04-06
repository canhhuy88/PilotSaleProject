namespace BizI.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
}
