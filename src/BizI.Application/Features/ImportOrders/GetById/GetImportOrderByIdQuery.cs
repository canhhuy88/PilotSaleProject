using BizI.Application.Features.ImportOrders.Dtos;

namespace BizI.Application.Features.ImportOrders.GetById;

public record GetImportOrderByIdQuery(Guid Id) : IRequest<ImportOrderDto?>;
