using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetReturnOrderById;

public record GetReturnOrderByIdQuery(Guid Id) : IRequest<ReturnOrderReadDto?>;
