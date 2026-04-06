using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.CustomerGroups;

public record GetCustomerGroupByIdQuery(string Id) : IRequest<CustomerGroup?>;

public class GetCustomerGroupByIdHandler : IRequestHandler<GetCustomerGroupByIdQuery, CustomerGroup?>
{
    private readonly IRepository<CustomerGroup> _customerGroupRepo;

    public GetCustomerGroupByIdHandler(IRepository<CustomerGroup> customerGroupRepo)
    {
        _customerGroupRepo = customerGroupRepo;
    }

    public async Task<CustomerGroup?> Handle(GetCustomerGroupByIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerGroupRepo.GetByIdAsync(request.Id);
    }
}
