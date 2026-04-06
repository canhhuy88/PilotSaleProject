using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetAllPayments;

public record GetAllPaymentsQuery : IRequest<IEnumerable<PaymentDto>>;
