namespace BizI.Application.Features.CustomerGroups.Delete;

public class DeleteCustomerGroupHandler : IRequestHandler<DeleteCustomerGroupCommand, CommandResult>
{
    private readonly IRepository<CustomerGroup> _repo;

    public DeleteCustomerGroupHandler(IRepository<CustomerGroup> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _repo.GetByIdAsync(request.Id);
        if (group is null) return CommandResult.FailureResult($"CustomerGroup '{request.Id}' not found.");
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
