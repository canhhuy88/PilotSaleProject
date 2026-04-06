namespace BizI.Api.Endpoints;

/// <summary>
/// Inventory stock-level endpoints (GET/import/export/return/adjust).
/// All business logic is in Application.Features.Inventory via MediatR.
/// </summary>
public static class InventoryEndpoints
{
    public static void MapInventoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/inventory").WithTags("Inventory");

        // GET /api/inventory — list all inventory records
        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllInventoryQuery())));

        // GET /api/inventory/product/{productId} — stock by product
        group.MapGet("/product/{productId}", async (string productId, IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetInventoryByProductQuery(productId))));

        // GET /api/inventory/warehouse/{warehouseId} — stock by warehouse
        group.MapGet("/warehouse/{warehouseId}", async (string warehouseId, IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetInventoryByWarehouseQuery(warehouseId))));

        // POST /api/inventory/import
        group.MapPost("/import", async (ImportStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        // POST /api/inventory/export
        group.MapPost("/export", async (ExportStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        // POST /api/inventory/return
        group.MapPost("/return", async (ReturnStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        // POST /api/inventory/adjust
        group.MapPost("/adjust", async (AdjustStockCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });
    }
}
