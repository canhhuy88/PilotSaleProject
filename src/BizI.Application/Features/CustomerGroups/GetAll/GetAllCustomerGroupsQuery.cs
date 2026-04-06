using BizI.Application.Features.CustomerGroups.Dtos;

namespace BizI.Application.Features.CustomerGroups.GetAll;

public record GetAllCustomerGroupsQuery : IRequest<IEnumerable<CustomerGroupDto>>;
