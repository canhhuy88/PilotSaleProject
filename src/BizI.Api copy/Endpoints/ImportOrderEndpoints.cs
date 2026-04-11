namespace BizI.Api.Endpoints;

public static class ImportOrderEndpoints
{
    public static void MapImportOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/import-orders").WithTags("ImportOrders");

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllImportOrdersQuery())));

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetImportOrderByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateImportOrderCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/import-orders/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}/confirm", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new ConfirmImportOrderCommand(id));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}/receive", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new ReceiveImportOrderCommand(id));
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteImportOrderCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
