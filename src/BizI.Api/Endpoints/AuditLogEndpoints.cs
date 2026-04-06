using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using BizI.Application.Features.AuditLogs;

namespace BizI.Api.Endpoints;

public class AuditLogEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/audit-logs").WithTags("AuditLogs");

        group.MapGet("/", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllAuditLogsQuery())));

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetAuditLogByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateAuditLogCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/audit-logs/{result.Id}", result) : Results.BadRequest(result.Message);
        });
        // Deliberately skipping Update/Delete for AuditLog as they are immutable
    }
}
