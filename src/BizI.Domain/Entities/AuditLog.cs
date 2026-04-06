namespace BizI.Domain.Entities;

public class AuditLog : BaseEntity
{
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string OldData { get; set; } = string.Empty; // JSON
    public string NewData { get; set; } = string.Empty; // JSON
    public string CreatedBy { get; set; } = string.Empty;
}