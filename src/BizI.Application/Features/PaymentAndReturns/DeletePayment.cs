using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.PaymentAndReturns;

public record DeletePaymentCommand(string Id) : IRequest<CommandResult>;

public class DeletePaymentHandler : IRequestHandler<DeletePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _repo;

    public DeletePaymentHandler(IRepository<Payment> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(DeletePaymentCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
