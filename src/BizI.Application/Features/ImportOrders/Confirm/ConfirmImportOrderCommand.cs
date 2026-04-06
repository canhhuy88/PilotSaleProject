using BizI.Application.Common;

namespace BizI.Application.Features.ImportOrders.Confirm;

public record ConfirmImportOrderCommand(Guid Id) : IRequest<CommandResult>;
