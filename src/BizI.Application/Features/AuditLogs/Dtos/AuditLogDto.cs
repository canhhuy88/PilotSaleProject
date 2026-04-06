namespace BizI.Application.Features.AuditLogs.Dtos;

public record AuditLogDto(
    Guid Id,
    string Action,
    string EntityName,
    Guid EntityId,
    string OldData,
    string NewData,
    string CreatedBy,
    DateTime CreatedAt);
