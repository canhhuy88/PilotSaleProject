using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetDebtById;

public record GetDebtByIdQuery(Guid Id) : IRequest<DebtDto?>;
