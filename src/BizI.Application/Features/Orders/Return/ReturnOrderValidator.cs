using BizI.Application.Features.Orders.Dtos;

namespace BizI.Application.Features.Orders.Return;

public class ReturnOrderValidator : AbstractValidator<ReturnOrderCommand>
{
    public ReturnOrderValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Items).NotEmpty();

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(x => x.ProductId).NotEmpty();
            item.RuleFor(x => x.Quantity).GreaterThan(0);
        });
    }
}
