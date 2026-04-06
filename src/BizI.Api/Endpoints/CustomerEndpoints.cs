namespace BizI.Api.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers").WithTags("Customers");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var customers = await mediator.Send(new GetAllCustomersQuery());
            return Results.Ok(customers);
        });

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var customer = await mediator.Send(new GetCustomerByIdQuery(id));
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        });

        group.MapPost("/", async (CreateCustomerCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/customers/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateCustomerCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCustomerCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
