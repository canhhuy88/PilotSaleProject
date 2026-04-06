namespace BizI.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public string BranchId { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
