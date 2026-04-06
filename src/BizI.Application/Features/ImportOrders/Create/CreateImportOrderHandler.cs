using BizI.Application.Common;
using BizI.Application.Features.ImportOrders.Dtos;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders.Create;

public class CreateImportOrderHandler : IRequestHandler<CreateImportOrderCommand, CommandResult>
{
    private readonly IRepository<ImportOrder> _repo;

    public CreateImportOrderHandler(IRepository<ImportOrder> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreateImportOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(i =>
            ImportOrderItem.Create(i.ProductId, i.Quantity, i.CostPrice, i.Currency));

        var order = ImportOrder.Create(request.SupplierId, items);
        await _repo.AddAsync(order);
        return CommandResult.SuccessResult(order.Id.ToString());
    }
}
