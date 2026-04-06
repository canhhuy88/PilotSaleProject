using BizI.Application.Features.Orders.Dtos;

namespace BizI.Application.Features.Orders.GetAll;

public record GetAllOrdersQuery : IRequest<IEnumerable<OrderDto>>;
