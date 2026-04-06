using BizI.Application.Features.ImportOrders.Dtos;

namespace BizI.Application.Features.ImportOrders.GetAll;

public record GetAllImportOrdersQuery : IRequest<IEnumerable<ImportOrderDto>>;
