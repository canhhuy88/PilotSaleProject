namespace BizI.Api.Endpoints;

public static class AuditLogEndpoints
{
    public static void MapAuditLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/audit-logs").WithTags("AuditLogs");

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllAuditLogsQuery())));

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetAuditLogByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        // AuditLogs are append-only; no Update or Delete endpoints
        group.MapPost("/", async (CreateAuditLogCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/audit-logs/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });
    }
}
