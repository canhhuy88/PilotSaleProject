namespace BizI.Application.Features.PaymentAndReturns.CreateDebt;

public class CreateDebtValidator : AbstractValidator<CreateDebtCommand>
{
    public CreateDebtValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
    }
}
