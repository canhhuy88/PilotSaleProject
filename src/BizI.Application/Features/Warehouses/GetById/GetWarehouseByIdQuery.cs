using BizI.Application.Features.Warehouses.Dtos;

namespace BizI.Application.Features.Warehouses.GetById;

public record GetWarehouseByIdQuery(Guid Id) : IRequest<WarehouseDto?>;
