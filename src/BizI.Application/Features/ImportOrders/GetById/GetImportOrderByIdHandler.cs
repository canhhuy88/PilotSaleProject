using AutoMapper;
using BizI.Application.Features.ImportOrders.Dtos;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.ImportOrders.GetById;

public class GetImportOrderByIdHandler : IRequestHandler<GetImportOrderByIdQuery, ImportOrderDto?>
{
    private readonly IRepository<BizI.Domain.Entities.ImportOrder> _repo;
    private readonly IMapper _mapper;

    public GetImportOrderByIdHandler(IRepository<BizI.Domain.Entities.ImportOrder> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ImportOrderDto?> Handle(GetImportOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _repo.GetByIdAsync(request.Id);
        return item is null ? null : _mapper.Map<ImportOrderDto>(item);
    }
}
