using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using BizI.Application.Features.StockTransactions;

namespace BizI.Api.Endpoints;

public class StockTransactionEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stock-transactions").WithTags("StockTransactions");

        group.MapGet("/", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllStockTransactionsQuery())));

        group.MapGet("/{id}", async (string id, IMediator mediator) =>
        {
            var res = await mediator.Send(new GetStockTransactionByIdQuery(id));
            return res is not null ? Results.Ok(res) : Results.NotFound();
        });

        group.MapPost("/", async (CreateStockTransactionCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Created($"/api/stock-transactions/{result.Id}", result) : Results.BadRequest(result.Message);
        });

        group.MapPut("/{id}", async (string id, UpdateStockTransactionCommand command, IMediator mediator) =>
        {
            if (id != command.Id) return Results.BadRequest("Id mismatch");
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });
        // Deliberately skipped Delete for Transactions (immutable log usually)
    }
}
