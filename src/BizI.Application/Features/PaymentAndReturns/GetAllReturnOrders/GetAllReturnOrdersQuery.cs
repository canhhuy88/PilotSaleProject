using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetAllReturnOrders;

public record GetAllReturnOrdersQuery : IRequest<IEnumerable<ReturnOrderReadDto>>;
