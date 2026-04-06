using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.PaymentAndReturns;

public record UpdatePaymentCommand(string Id, string OrderId, decimal Amount, string Method) : IRequest<CommandResult>;

public class UpdatePaymentCommandValidator : AbstractValidator<UpdatePaymentCommand>
{
    public UpdatePaymentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
    }
}

public class UpdatePaymentHandler : IRequestHandler<UpdatePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _repo;

    public UpdatePaymentHandler(IRepository<Payment> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(UpdatePaymentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.OrderId = request.OrderId;
        entity.Amount = request.Amount;
        entity.Method = request.Method;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
