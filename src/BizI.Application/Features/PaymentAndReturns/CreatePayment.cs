using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.PaymentAndReturns;

public record CreatePaymentCommand(string OrderId, decimal Amount, string Method) : IRequest<CommandResult>;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _repo;

    public CreatePaymentHandler(IRepository<Payment> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = new Payment { OrderId = request.OrderId, Amount = request.Amount, Method = request.Method };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
