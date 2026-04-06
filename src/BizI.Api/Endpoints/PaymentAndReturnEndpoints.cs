using BizI.Application.Features.PaymentAndReturns;
using MediatR;

namespace BizI.Api.Endpoints;

/// <summary>
/// Minimal API endpoints for Payment, Debt, and ReturnOrder resources.
/// Thin endpoints — all business logic in Application layer.
/// </summary>
public static class PaymentAndReturnEndpoints
{
    public static void MapPaymentAndReturnEndpoints(this IEndpointRouteBuilder app)
    {
        // ── Payments ──────────────────────────────────────────────────────────
        var payments = app.MapGroup("/api/payments").WithTags("Payments");

        payments.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllPaymentsQuery())));

        payments.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var p = await mediator.Send(new GetPaymentByIdQuery(id));
            return p is not null ? Results.Ok(p) : Results.NotFound();
        });

        payments.MapPost("/", async (CreatePaymentCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/payments/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        payments.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePaymentCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });

        // ── Debts ─────────────────────────────────────────────────────────────
        var debts = app.MapGroup("/api/debts").WithTags("Debts");

        debts.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllDebtsQuery())));

        debts.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var d = await mediator.Send(new GetDebtByIdQuery(id));
            return d is not null ? Results.Ok(d) : Results.NotFound();
        });

        debts.MapPost("/", async (CreateDebtCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/debts/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        debts.MapPost("/{id}/pay", async (string id, RecordDebtPaymentCommand command, IMediator mediator) =>
        {
            if (id != command.DebtId) return Results.BadRequest("Route id does not match body DebtId.");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        debts.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteDebtCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });

        // ── Return Orders ─────────────────────────────────────────────────────
        var returns = app.MapGroup("/api/returns").WithTags("Returns");

        returns.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllReturnOrdersQuery())));

        returns.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var ro = await mediator.Send(new GetReturnOrderByIdQuery(id));
            return ro is not null ? Results.Ok(ro) : Results.NotFound();
        });

        returns.MapPost("/", async (CreateReturnOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/returns/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        returns.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteReturnOrderCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
