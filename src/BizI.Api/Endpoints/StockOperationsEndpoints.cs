using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using BizI.Application.Features.StockOperations;

namespace BizI.Api.Endpoints;

public class StockOperationsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stock-operations").WithTags("StockOperations");

        group.MapPost("/in", async (CreateStockInCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapPost("/out", async (CreateStockOutCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapPost("/transfer", async (CreateStockTransferCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        group.MapPost("/audit", async (CreateStockAuditCommand command, IMediator mediator) =>
        {
            var result = await mediator.Send(command);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result.Message);
        });

        // GetAll Endpoints
        group.MapGet("/in", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllStockInsQuery())));
        group.MapGet("/out", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllStockOutsQuery())));
        group.MapGet("/transfer", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllStockTransfersQuery())));
        group.MapGet("/audit", async (IMediator mediator) => Results.Ok(await mediator.Send(new GetAllStockAuditsQuery())));
    }
}
