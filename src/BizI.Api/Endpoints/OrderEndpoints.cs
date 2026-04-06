using BizI.Application.DTOs.Order;
using BizI.Application.Features.Orders;
using MediatR;

namespace BizI.Api.Endpoints;

/// <summary>
/// Minimal API endpoints for Order operations.
/// Thin endpoints — all logic in Application layer.
/// </summary>
public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        // GET /api/orders
        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllOrdersQuery())));

        // GET /api/orders/{id}
        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetOrderQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        // POST /api/orders
        group.MapPost("/", async (CreateOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/orders/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        // DELETE /api/orders/{id} — cancels the order
        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteOrderCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });

        // POST /api/orders/{id}/return
        group.MapPost("/{id}/return", async (string id, ReturnOrderCommand command, IMediator mediator) =>
        {
            if (id != command.OrderId) return Results.BadRequest("Route id does not match body OrderId.");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });
    }
}
