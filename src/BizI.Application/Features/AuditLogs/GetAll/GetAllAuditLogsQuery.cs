using BizI.Application.Features.AuditLogs.Dtos;

namespace BizI.Application.Features.AuditLogs.GetAll;

public record GetAllAuditLogsQuery : IRequest<IEnumerable<AuditLogDto>>;
