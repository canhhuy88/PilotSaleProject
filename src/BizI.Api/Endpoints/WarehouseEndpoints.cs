using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BizI.Application.Features.Warehouses;

namespace BizI.Api.Endpoints;

public class WarehouseEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/warehouses").WithTags("Warehouses");

        group.MapGet("/", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllWarehousesQuery())));

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetWarehouseByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateWarehouseCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/warehouses/{result.Id}", result) : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateWarehouseCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteWarehouseCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
