using BizI.Application.Features.AuditLogs.Dtos;

namespace BizI.Application.Features.AuditLogs.GetById;

public record GetAuditLogByIdQuery(Guid Id) : IRequest<AuditLogDto?>;
