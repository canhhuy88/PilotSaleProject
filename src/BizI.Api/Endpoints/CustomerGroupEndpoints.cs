using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BizI.Application.Features.CustomerGroups;

namespace BizI.Api.Endpoints;

public class CustomerGroupEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customer-groups").WithTags("CustomerGroups");

        group.MapGet("/", async (IMediator mediator, ILogger<CustomerGroupEndpoints> logger) =>
        {
            logger.LogInformation("Fetching all customer groups.");
            var groups = await mediator.Send(new GetAllCustomerGroupsQuery());
            return Results.Ok(groups);
        });

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var groupEntity = await mediator.Send(new GetCustomerGroupByIdQuery(id));
            return groupEntity is not null ? Results.Ok(groupEntity) : Results.NotFound();
        });

        group.MapPost("/", async (CreateCustomerGroupCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success
                ? Results.Created($"/api/customer-groups/{result.Id}", result)
                : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateCustomerGroupCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");

            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapDelete("/{id}", async (string id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteCustomerGroupCommand(id));
            return result.Success ? Results.NoContent() : Results.BadRequest(result.Message);
        });
    }
}
