using BizI.Application.Features.PaymentAndReturns.Dtos;

namespace BizI.Application.Features.PaymentAndReturns.GetAllDebts;

public record GetAllDebtsQuery : IRequest<IEnumerable<DebtDto>>;
