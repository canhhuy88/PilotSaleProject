using BizI.Application.Features.Customers.Dtos;

namespace BizI.Application.Features.Customers.GetAll;

public record GetAllCustomersQuery : IRequest<IEnumerable<CustomerDto>>;
