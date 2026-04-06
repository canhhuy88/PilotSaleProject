namespace BizI.Application.Features.CustomerGroups.Create;

public class CreateCustomerGroupValidator : AbstractValidator<CreateCustomerGroupCommand>
{
    public CreateCustomerGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
    }
}
