namespace BizI.Application.Features.Orders;

public record GetOrderQuery(Guid OrderId) : IRequest<Order?>;

public class GetOrderHandler : IRequestHandler<GetOrderQuery, Order?>
{
    private readonly IRepository<Order> _orderRepo;

    public GetOrderHandler(IRepository<Order> orderRepo)
    {
        _orderRepo = orderRepo;
    }

    public async Task<Order?> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        return await _orderRepo.GetByIdAsync(request.OrderId.ToString());
    }
}
