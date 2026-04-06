using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using BizI.Application.Features.PaymentAndReturns;

namespace BizI.Api.Endpoints;

public class PaymentAndReturnEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/transactions").WithTags("PaymentAndReturns");

        // Payments
        group.MapGet("/payments", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllPaymentsQuery())));
        group.MapGet("/payments/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetPaymentByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });
        group.MapPost("/payments", async (CreatePaymentCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/transactions/payments/{result.Id}", result) : Results.BadRequest(result.Message);
        });
        group.MapPut("/payments/{id}", async (string id, UpdatePaymentCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });
        group.MapDelete("/payments/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeletePaymentCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });

        // Debts
        group.MapGet("/debts", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllDebtsQuery())));
        group.MapGet("/debts/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetDebtByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });
        group.MapPost("/debts", async (CreateDebtCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/transactions/debts/{result.Id}", result) : Results.BadRequest(result.Message);
        });
        group.MapPut("/debts/{id}", async (string id, UpdateDebtCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });
        group.MapDelete("/debts/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteDebtCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });

        // Return Orders
        group.MapGet("/returns", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllReturnOrdersQuery())));
        group.MapGet("/returns/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetReturnOrderByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });
        group.MapPost("/returns", async (CreateReturnOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/transactions/returns/{result.Id}", result) : Results.BadRequest(result.Message);
        });
        group.MapDelete("/returns/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteReturnOrderCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
