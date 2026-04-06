using BizI.Domain.Exceptions;

namespace BizI.Domain.Entities;

/// <summary>
/// Represents an immutable audit trail entry.
/// Once created, audit logs cannot be mutated — they are append-only records.
/// </summary>
public class AuditLog : BaseEntity
{
    public string Action { get; private set; } = string.Empty;
    public string EntityName { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public string OldData { get; private set; } = string.Empty;   // JSON snapshot
    public string NewData { get; private set; } = string.Empty;   // JSON snapshot
    public string CreatedBy { get; private set; } = string.Empty;

    private AuditLog() { } // ORM / serialization

    /// <summary>
    /// Factory method — the only valid way to create an audit log entry.
    /// Enforces that Action, EntityName, and EntityId are always present.
    /// </summary>
    public static AuditLog Create(
        string action,
        string entityName,
        string entityId,
        string createdBy,
        string oldData = "",
        string newData = "")
    {
        if (string.IsNullOrWhiteSpace(action))
            throw new DomainException("Audit log action cannot be empty.");

        if (string.IsNullOrWhiteSpace(entityName))
            throw new DomainException("Audit log entity name cannot be empty.");

        if (string.IsNullOrWhiteSpace(entityId))
            throw new DomainException("Audit log entity ID cannot be empty.");

        return new AuditLog
        {
            Action = action.Trim(),
            EntityName = entityName.Trim(),
            EntityId = entityId.Trim(),
            CreatedBy = createdBy.Trim(),
            OldData = oldData,
            NewData = newData
        };
    }
}
