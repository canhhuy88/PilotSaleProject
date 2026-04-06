using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.CreateReturnOrder;

public class CreateReturnOrderValidator : AbstractValidator<CreateReturnOrderCommand>
{
    public CreateReturnOrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();
        RuleForEach(x => x.Items).ChildRules(i =>
        {
            i.RuleFor(x => x.ProductId).NotEmpty();
            i.RuleFor(x => x.Quantity).GreaterThan(0);
            i.RuleFor(x => x.RefundPrice).GreaterThanOrEqualTo(0);
        });
    }
}
