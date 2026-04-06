using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Customers;

public record GetAllCustomersQuery() : IRequest<IEnumerable<Customer>>;

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<Customer>>
{
    private readonly IRepository<Customer> _customerRepo;

    public GetAllCustomersHandler(IRepository<Customer> customerRepo)
    {
        _customerRepo = customerRepo;
    }

    public async Task<IEnumerable<Customer>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return await _customerRepo.GetAllAsync();
    }
}
