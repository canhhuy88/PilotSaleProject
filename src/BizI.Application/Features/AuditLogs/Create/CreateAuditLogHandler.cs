using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.AuditLogs.Create;

public class CreateAuditLogHandler : IRequestHandler<CreateAuditLogCommand, CommandResult>
{
    private readonly IRepository<AuditLog> _auditRepo;
    private readonly ILogger<CreateAuditLogHandler> _logger;

    public CreateAuditLogHandler(IRepository<AuditLog> auditRepo, ILogger<CreateAuditLogHandler> logger)
    {
        _auditRepo = auditRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Use Domain factory — AuditLog is append-only
            var log = AuditLog.Create(
                request.Action,
                request.EntityName,
                request.EntityId,
                request.CreatedBy,
                request.OldData,
                request.NewData);

            await _auditRepo.AddAsync(log);
            _logger.LogInformation("Audit log created. Id: {LogId}, Action: {Action}", log.Id, log.Action);
            return CommandResult.SuccessResult(log.Id);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Domain rule violated creating audit log: {Message}", ex.Message);
            return CommandResult.FailureResult(ex.Message);
        }
    }
}
