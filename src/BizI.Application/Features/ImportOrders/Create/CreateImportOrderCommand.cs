using BizI.Application.Common;
using BizI.Application.Features.ImportOrders.Dtos;

namespace BizI.Application.Features.ImportOrders.Create;

public record CreateImportOrderCommand(
    Guid SupplierId,
    List<CreateImportOrderItemDto> Items) : IRequest<CommandResult>;
