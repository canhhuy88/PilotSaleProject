using BizI.Application.Features.Customers.Dtos;

namespace BizI.Application.Features.Customers.GetAll;

public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, IEnumerable<CustomerDto>>
{
    private readonly IRepository<Customer> _customerRepo;
    private readonly IMapper _mapper;

    public GetAllCustomersHandler(IRepository<Customer> customerRepo, IMapper mapper)
    {
        _customerRepo = customerRepo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        var customers = await _customerRepo.GetAllAsync();

        // ✅ Return DTOs — never raw entities
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }
}
