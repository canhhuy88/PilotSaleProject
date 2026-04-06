using BizI.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.AuditLogs;

// ── DTOs ─────────────────────────────────────────────────────────────────────

public record AuditLogDto(
    string Id,
    string Action,
    string EntityName,
    string EntityId,
    string OldData,
    string NewData,
    string CreatedBy,
    DateTime CreatedAt);

// ── Commands/Queries ──────────────────────────────────────────────────────────

public record CreateAuditLogCommand(
    string Action,
    string EntityName,
    string EntityId,
    string CreatedBy,
    string OldData = "",
    string NewData = "") : IRequest<CommandResult>;

public record GetAllAuditLogsQuery : IRequest<IEnumerable<AuditLogDto>>;

public record GetAuditLogByIdQuery(string Id) : IRequest<AuditLogDto?>;

// ── Validators ────────────────────────────────────────────────────────────────

public class CreateAuditLogCommandValidator : AbstractValidator<CreateAuditLogCommand>
{
    public CreateAuditLogCommandValidator()
    {
        RuleFor(x => x.Action).NotEmpty();
        RuleFor(x => x.EntityName).NotEmpty();
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.CreatedBy).NotEmpty();
    }
}

// ── Handlers ──────────────────────────────────────────────────────────────────

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
