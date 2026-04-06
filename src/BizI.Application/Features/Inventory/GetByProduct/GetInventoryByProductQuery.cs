using BizI.Application.Features.Inventory.Dtos;

namespace BizI.Application.Features.Inventory.GetByProduct;

public record GetInventoryByProductQuery(Guid ProductId) : IRequest<IEnumerable<InventoryDto>>;
