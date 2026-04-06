using AutoMapper;
using BizI.Application.Features.ImportOrders.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders.GetAll;

public class GetAllImportOrdersHandler : IRequestHandler<GetAllImportOrdersQuery, IEnumerable<ImportOrderDto>>
{
    private readonly IRepository<BizI.Domain.Entities.ImportOrder> _repo;
    private readonly IMapper _mapper;

    public GetAllImportOrdersHandler(IRepository<BizI.Domain.Entities.ImportOrder> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ImportOrderDto>> Handle(GetAllImportOrdersQuery request, CancellationToken cancellationToken)
    {
        var items = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<ImportOrderDto>>(items);
    }
}
