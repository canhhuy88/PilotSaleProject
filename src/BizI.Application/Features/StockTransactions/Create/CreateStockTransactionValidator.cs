namespace BizI.Application.Features.StockTransactions.Create;

public class CreateStockTransactionValidator : AbstractValidator<CreateStockTransactionCommand>
{
    public CreateStockTransactionValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.WarehouseId).NotEmpty();
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        RuleFor(x => x.BeforeQty).GreaterThanOrEqualTo(0);
    }
}
