using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.AuditLogs;

public record CreateAuditLogCommand(
    string Action,
    string EntityName,
    string EntityId,
    string OldData,
    string NewData,
    string CreatedBy
) : IRequest<CommandResult>;

public class CreateAuditLogCommandValidator : AbstractValidator<CreateAuditLogCommand>
{
    public CreateAuditLogCommandValidator()
    {
        RuleFor(x => x.EntityName).NotEmpty();
        RuleFor(x => x.Action).NotEmpty();
    }
}

public class CreateAuditLogHandler : IRequestHandler<CreateAuditLogCommand, CommandResult>
{
    private readonly IRepository<AuditLog> _repo;

    public CreateAuditLogHandler(IRepository<AuditLog> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
    {
        var entity = new AuditLog
        {
            Action = request.Action,
            EntityName = request.EntityName,
            EntityId = request.EntityId,
            OldData = request.OldData,
            NewData = request.NewData,
            CreatedBy = request.CreatedBy
        };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
