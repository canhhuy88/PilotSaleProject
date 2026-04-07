namespace BizI.Api.Endpoints;

public static class StockTransactionEndpoints
{
    public static void MapStockTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stock-transactions").WithTags("StockTransactions");

        group.MapPost("/", async (CreateStockTransactionCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/stock-transactions/{result.Id}", result) : Results.BadRequest(result.Message);
        });

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllStockTransactionsQuery())));

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetStockTransactionByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
