using BizI.Application.Features.Products;
using MediatR;

namespace BizI.Api.Endpoints;

/// <summary>
/// Minimal API endpoints for the Product resource.
/// Thin endpoints — all business logic lives in Application layer.
/// </summary>
public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithTags("Products");

        // GET /api/products
        group.MapGet("/", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetAllProductsQuery())));

        // GET /api/products/{id}
        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var product = await mediator.Send(new GetProductByIdQuery(id));
            return product is not null ? Results.Ok(product) : Results.NotFound();
        });

        // POST /api/products
        group.MapPost("/", async (CreateProductCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/products/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        // PUT /api/products/{id}
        group.MapPut("/{id}", async (Guid id, UpdateProductCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Route id does not match body id.");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        // DELETE /api/products/{id}
        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteProductCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
