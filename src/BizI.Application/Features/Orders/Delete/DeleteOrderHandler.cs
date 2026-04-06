using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Orders.Delete;

public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, CommandResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DeleteOrderHandler> _logger;

    public DeleteOrderHandler(IOrderRepository orderRepository, ILogger<DeleteOrderHandler> logger)
    {
        _orderRepository = orderRepository;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.Id);
        if (order is null)
            return CommandResult.FailureResult($"Order '{request.Id}' not found.");

        // ✅ Use Domain method to enforce cancellation rules
        try { order.Cancel(); }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }

        await _orderRepository.UpdateAsync(order);
        _logger.LogInformation("Order cancelled/deleted. Id: {Id}", request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
