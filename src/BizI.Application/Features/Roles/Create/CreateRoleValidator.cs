namespace BizI.Application.Features.Roles.Create;

public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleValidator() { RuleFor(x => x.Name).NotEmpty().MaximumLength(100); }
}
