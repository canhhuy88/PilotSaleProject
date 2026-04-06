using BizI.Application.Features.CustomerGroups.Dtos;

namespace BizI.Application.Features.CustomerGroups.GetById;

public record GetCustomerGroupByIdQuery(Guid Id) : IRequest<CustomerGroupDto?>;
