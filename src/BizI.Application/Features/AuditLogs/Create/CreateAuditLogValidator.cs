namespace BizI.Application.Features.AuditLogs.Create;

public class CreateAuditLogValidator : AbstractValidator<CreateAuditLogCommand>
{
    public CreateAuditLogValidator()
    {
        RuleFor(x => x.Action).NotEmpty();
        RuleFor(x => x.EntityName).NotEmpty();
        RuleFor(x => x.EntityId).NotEmpty();
        RuleFor(x => x.CreatedBy).NotEmpty();
    }
}
