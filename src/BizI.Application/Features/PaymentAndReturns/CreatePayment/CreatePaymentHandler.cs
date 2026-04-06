using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.PaymentAndReturns.CreatePayment;

public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CommandResult>
{
    private readonly IRepository<Payment> _paymentRepo;
    private readonly ILogger<CreatePaymentHandler> _logger;

    public CreatePaymentHandler(IRepository<Payment> paymentRepo, ILogger<CreatePaymentHandler> logger)
    {
        _paymentRepo = paymentRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Domain factory
            var payment = Payment.Create(request.OrderId, request.Amount, request.Method, request.Currency);
            await _paymentRepo.AddAsync(payment);
            _logger.LogInformation("Payment created. Id: {Id}", payment.Id);
            return CommandResult.SuccessResult(payment.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
