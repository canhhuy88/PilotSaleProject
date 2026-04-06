namespace BizI.Application.Features.CustomerGroups.Update;

public class UpdateCustomerGroupHandler : IRequestHandler<UpdateCustomerGroupCommand, CommandResult>
{
    private readonly IRepository<CustomerGroup> _repo;

    public UpdateCustomerGroupHandler(IRepository<CustomerGroup> repo) => _repo = repo;

    public async Task<CommandResult> Handle(UpdateCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        var group = await _repo.GetByIdAsync(request.Id);
        if (group is null) return CommandResult.FailureResult($"CustomerGroup '{request.Id}' not found.");
        try
        {
            group.Rename(request.Name);
            group.SetDiscount(request.DiscountPercent);
            await _repo.UpdateAsync(group);
            return CommandResult.SuccessResult(group.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
