namespace BizI.Api.Endpoints;

/// <summary>
/// Inventory Transaction log endpoints (read-only query surface).
/// Write operations go through InventoryEndpoints (import/export/adjust/return).
/// </summary>
public static class InventoryTransactionEndpoints
{
    public static void MapInventoryTransactionEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory-transactions").WithTags("InventoryTransactions");

        // GET /api/inventory-transactions
        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllInventoryTransactionsQuery())));

        // GET /api/inventory-transactions/{id}
        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetInventoryTransactionByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });
    }
}
