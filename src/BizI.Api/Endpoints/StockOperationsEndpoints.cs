namespace BizI.Api.Endpoints;

public static class StockOperationsEndpoints
{
    public static void MapStockOperationsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stock-operations").WithTags("StockOperations");

        group.MapPost("/", async (CreateStockOperationCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/stock-operations/{result.Id}", result) : Results.BadRequest(result.Message);
        });

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllStockOperationsQuery())));

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStockOperationByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
