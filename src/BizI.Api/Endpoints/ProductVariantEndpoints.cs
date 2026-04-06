using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BizI.Application.Features.ProductVariants;

namespace BizI.Api.Endpoints;

public class ProductVariantEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/product-variants").WithTags("ProductVariants");

        group.MapGet("/", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllProductVariantsQuery())));

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetProductVariantByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateProductVariantCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/product-variants/{result.Id}", result) : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateProductVariantCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteProductVariantCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
