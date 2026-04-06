using BizI.Application.Common;

namespace BizI.Application.Features.ImportOrders.Delete;

public record DeleteImportOrderCommand(Guid Id) : IRequest<CommandResult>;
