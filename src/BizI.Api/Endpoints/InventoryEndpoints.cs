using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BizI.Application.Features.Inventory;

namespace BizI.Api.Endpoints;

public class InventoryEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory").WithTags("Inventory");

        group.MapGet("/", async (System.Guid? productId, System.Guid? warehouseId, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetInventoryQuery(productId, warehouseId));
            return Results.Ok(res);
        });

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetInventoryByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/import", async (ImportStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok() : Results.BadRequest(result.Message);
        });

        group.MapPost("/adjust", async (AdjustStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok() : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateInventoryCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteInventoryCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
