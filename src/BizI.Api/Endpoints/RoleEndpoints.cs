using BizI.Application.Features.Roles;
using MediatR;

namespace BizI.Api.Endpoints;

/// <summary>
/// Minimal API endpoints for Role resource.
/// </summary>
public static class RoleEndpoints
{
    public static void MapRoleEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/roles").WithTags("Roles").RequireAuthorization();

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllRolesQuery())));

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var role = await mediator.Send(new GetRoleByIdQuery(id));
            return role is not null ? Results.Ok(role) : Results.NotFound();
        });

        group.MapPost("/", async (CreateRoleCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/roles/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateRoleCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Route id does not match body id.");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteRoleCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
