using BizI.Application.Features.AuditLogs.Dtos;

namespace BizI.Application.Features.AuditLogs.GetAll;

public class GetAllAuditLogsHandler : IRequestHandler<GetAllAuditLogsQuery, IEnumerable<AuditLogDto>>
{
    private readonly IRepository<AuditLog> _auditRepo;

    public GetAllAuditLogsHandler(IRepository<AuditLog> auditRepo) => _auditRepo = auditRepo;

    public async Task<IEnumerable<AuditLogDto>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
    {
        var logs = await _auditRepo.GetAllAsync();
        return logs.Select(l => new AuditLogDto(
            l.Id, l.Action, l.EntityName, l.EntityId,
            l.OldData, l.NewData, l.CreatedBy, l.CreatedAt));
    }
}
