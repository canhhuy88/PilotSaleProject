using BizI.Application.Common;

namespace BizI.Application.Features.ImportOrders.Receive;

public record ReceiveImportOrderCommand(Guid Id) : IRequest<CommandResult>;
