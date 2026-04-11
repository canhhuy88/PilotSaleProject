namespace BizI.Api.Endpoints;

public static class StockItemEndpoints
{
    public static void MapStockItemEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stock-items").WithTags("StockItems");

        group.MapPost("/", async (CreateStockItemCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/stock-items/{result.Id}", result) : Results.BadRequest(result.Message);
        });

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllStockItemsQuery())));

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStockItemByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPut("/{id}", async (Guid id, UpdateStockItemCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("ID mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteStockItemCommand(id));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });
    }
}
