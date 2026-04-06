using BizI.Application.Features.Customers.Dtos;

namespace BizI.Application.Features.Customers.GetById;

public record GetCustomerByIdQuery(Guid Id) : IRequest<CustomerDto?>;
