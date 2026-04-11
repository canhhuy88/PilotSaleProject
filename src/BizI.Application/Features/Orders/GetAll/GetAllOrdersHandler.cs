using BizI.Application.Features.Orders.Dtos;

namespace BizI.Application.Features.Orders.GetAll;

public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, IEnumerable<OrderDto>>
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IMapper _mapper;

    public GetAllOrdersHandler(IRepository<Order> orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await _orderRepository.GetAllAsync();

        // ✅ Return DTOs — never raw entities
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}
