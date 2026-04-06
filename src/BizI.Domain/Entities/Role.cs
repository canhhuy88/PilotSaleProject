namespace BizI.Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string[] Permissions { get; set; } = Array.Empty<string>();
}