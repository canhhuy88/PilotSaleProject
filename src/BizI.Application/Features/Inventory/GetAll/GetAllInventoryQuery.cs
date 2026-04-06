using BizI.Application.Features.Inventory.Dtos;

namespace BizI.Application.Features.Inventory.GetAll;

public record GetAllInventoryQuery : IRequest<IEnumerable<InventoryDto>>;
