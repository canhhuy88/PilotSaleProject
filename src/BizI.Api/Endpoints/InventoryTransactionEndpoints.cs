using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BizI.Application.Features.InventoryTransactions;

namespace BizI.Api.Endpoints;

public class InventoryTransactionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory-transactions").WithTags("InventoryTransactions");

        group.MapGet("/", async (IMediator mediator, ILogger<InventoryTransactionEndpoints> logger) =>
        {
            logger.LogInformation("Fetching all inventory transactions.");
            var trans = await mediator.Send(new GetAllInventoryTransactionsQuery());
            return Results.Ok(trans);
        });

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetInventoryTransactionByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateInventoryTransactionCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/inventory-transactions/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateInventoryTransactionCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");

            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteInventoryTransactionCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
