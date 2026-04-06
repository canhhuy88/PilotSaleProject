using BizI.Application.DTOs.CustomerGroup;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.CustomerGroups;

public record CreateCustomerGroupCommand(string Name, decimal DiscountPercent = 0m) : IRequest<CommandResult>;
public record UpdateCustomerGroupCommand(Guid Id, string Name, decimal DiscountPercent = 0m) : IRequest<CommandResult>;
public record DeleteCustomerGroupCommand(Guid Id) : IRequest<CommandResult>;
public record GetAllCustomerGroupsQuery : IRequest<IEnumerable<CustomerGroupDto>>;
public record GetCustomerGroupByIdQuery(Guid Id) : IRequest<CustomerGroupDto?>;

public class CreateCustomerGroupCommandValidator : AbstractValidator<CreateCustomerGroupCommand>
{
    public CreateCustomerGroupCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DiscountPercent).InclusiveBetween(0, 100);
    }
}

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

public class GetAllCustomerGroupsHandler : IRequestHandler<GetAllCustomerGroupsQuery, IEnumerable<CustomerGroupDto>>
{
    private readonly IRepository<CustomerGroup> _repo;
    private readonly IMapper _mapper;
    public GetAllCustomerGroupsHandler(IRepository<CustomerGroup> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<CustomerGroupDto>> Handle(GetAllCustomerGroupsQuery request, CancellationToken cancellationToken)
        => _mapper.Map<IEnumerable<CustomerGroupDto>>(await _repo.GetAllAsync());
}

public class GetCustomerGroupByIdHandler : IRequestHandler<GetCustomerGroupByIdQuery, CustomerGroupDto?>
{
    private readonly IRepository<CustomerGroup> _repo;
    private readonly IMapper _mapper;
    public GetCustomerGroupByIdHandler(IRepository<CustomerGroup> repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<CustomerGroupDto?> Handle(GetCustomerGroupByIdQuery request, CancellationToken cancellationToken)
    {
        var group = await _repo.GetByIdAsync(request.Id);
        return group is null ? null : _mapper.Map<CustomerGroupDto>(group);
    }
}
