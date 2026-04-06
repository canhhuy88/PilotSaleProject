using BizI.Application.Features.Customers.Dtos;

namespace BizI.Application.Features.Customers.GetById;

public class GetCustomerByIdHandler : IRequestHandler<GetCustomerByIdQuery, CustomerDto?>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;

    public GetCustomerByIdHandler(IRepository<Customer> customerRepo, IMapper mapper)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
    }

    public async Task<CustomerDto?> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepo.GetByIdAsync(request.Id);
        return customer is null ? null : _mapper.Map<CustomerDto>(customer);
    }
}
