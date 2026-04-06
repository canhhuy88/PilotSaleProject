namespace BizI.Api.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users").WithTags("Users");

        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllUsersQuery())));

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetUserByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateUserCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/users/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (Guid id, UpdateUserCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteUserCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
