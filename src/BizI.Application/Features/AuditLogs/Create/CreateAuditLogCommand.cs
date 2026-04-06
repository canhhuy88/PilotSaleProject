namespace BizI.Application.Features.AuditLogs.Create;

public record CreateAuditLogCommand(
    string Action,
    string EntityName,
    Guid EntityId,
    string CreatedBy,
    string OldData = "",
    string NewData = "") : IRequest<CommandResult>;
