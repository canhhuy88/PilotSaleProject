namespace BizI.Api.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories").WithTags("Categories");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var categories = await mediator.Send(new GetAllCategoriesQuery());
            return Results.Ok(categories);
        });

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var category = await mediator.Send(new GetCategoryByIdQuery(id));
            return category is not null ? Results.Ok(category) : Results.NotFound();
        });

        group.MapPost("/", async (CreateCategoryCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/categories/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateCategoryCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCategoryCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
