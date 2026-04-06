namespace BizI.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? ParentId { get; set; }
    public string? Description { get; set; }
}