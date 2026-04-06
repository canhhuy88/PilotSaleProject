namespace BizI.Application.Features.CustomerGroups.Create;

public class CreateCustomerGroupHandler : IRequestHandler<CreateCustomerGroupCommand, CommandResult>
{
    private readonly IRepository<CustomerGroup> _repo;

    public CreateCustomerGroupHandler(IRepository<CustomerGroup> repo) => _repo = repo;

    public async Task<CommandResult> Handle(CreateCustomerGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var group = CustomerGroup.Create(request.Name, request.DiscountPercent);
            await _repo.AddAsync(group);
            return CommandResult.SuccessResult(group.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
