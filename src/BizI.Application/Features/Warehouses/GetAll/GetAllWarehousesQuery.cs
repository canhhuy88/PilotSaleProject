using BizI.Application.Features.Warehouses.Dtos;

namespace BizI.Application.Features.Warehouses.GetAll;

public record GetAllWarehousesQuery : IRequest<IEnumerable<WarehouseDto>>;
