using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.CustomerGroups;

public record GetAllCustomerGroupsQuery() : IRequest<IEnumerable<CustomerGroup>>;

public class GetAllCustomerGroupsHandler : IRequestHandler<GetAllCustomerGroupsQuery, IEnumerable<CustomerGroup>>
{
    private readonly IRepository<CustomerGroup> _customerGroupRepo;

    public GetAllCustomerGroupsHandler(IRepository<CustomerGroup> customerGroupRepo)
    {
        _customerGroupRepo = customerGroupRepo;
    }

    public async Task<IEnumerable<CustomerGroup>> Handle(GetAllCustomerGroupsQuery request, CancellationToken cancellationToken)
    {
        return await _customerGroupRepo.GetAllAsync();
    }
}
