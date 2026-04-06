using BizI.Application.Features.Orders.Dtos;

namespace BizI.Application.Features.Orders.GetById;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto?>;
