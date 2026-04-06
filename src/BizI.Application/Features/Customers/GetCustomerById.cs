using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.Customers;

public record GetCustomerByIdQuery(string Id) : IRequest<Customer?>;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, Customer?>
{
    private readonly IRepository<Customer> _customerRepo;

    public GetCustomerByIdHandler(IRepository<Customer> customerRepo)
    {
        _customerRepo = customerRepo;
    }

    public async Task<Customer?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        return await _customerRepo.GetByIdAsync(request.Id);
    }
}
