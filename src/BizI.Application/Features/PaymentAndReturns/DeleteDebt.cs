using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.PaymentAndReturns;

public record DeleteDebtCommand(string Id) : IRequest<CommandResult>;

public class DeleteDebtHandler : IRequestHandler<DeleteDebtCommand, CommandResult>
{
    private readonly IRepository<Debt> _repo;

    public DeleteDebtHandler(IRepository<Debt> repo)
    {
        _repo = repo;
    }

    public async Task<CommandResult> Handle(DeleteDebtCommand request, CancellationToken cancellationToken)
    {
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
