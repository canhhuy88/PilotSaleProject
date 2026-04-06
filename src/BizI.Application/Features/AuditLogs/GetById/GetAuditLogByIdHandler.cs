using BizI.Application.Features.AuditLogs.Dtos;

namespace BizI.Application.Features.AuditLogs.GetById;

public class GetAuditLogByIdHandler : IRequestHandler<GetAuditLogByIdQuery, AuditLogDto?>
{
    private readonly IRepository<AuditLog> _auditRepo;

    public GetAuditLogByIdHandler(IRepository<AuditLog> auditRepo) => _auditRepo = auditRepo;

    public async Task<AuditLogDto?> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
    {
        var log = await _auditRepo.GetByIdAsync(request.Id);
        if (log is null) return null;
        return new AuditLogDto(
            log.Id, log.Action, log.EntityName, log.EntityId,
            log.OldData, log.NewData, log.CreatedBy, log.CreatedAt);
    }
}
