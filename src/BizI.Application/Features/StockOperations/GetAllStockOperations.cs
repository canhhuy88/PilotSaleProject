using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using System.Collections.Generic;

namespace BizI.Application.Features.StockOperations;

public record GetAllStockInsQuery() : IRequest<IEnumerable<StockIn>>;

public class GetAllStockInsHandler : IRequestHandler<GetAllStockInsQuery, IEnumerable<StockIn>>
{
    private readonly IRepository<StockIn> _repo;

    public GetAllStockInsHandler(IRepository<StockIn> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockIn>> Handle(GetAllStockInsQuery request, CancellationToken cancellationToken) => await _repo.GetAllAsync();
}

public record GetAllStockOutsQuery() : IRequest<IEnumerable<StockOut>>;

public class GetAllStockOutsHandler : IRequestHandler<GetAllStockOutsQuery, IEnumerable<StockOut>>
{
    private readonly IRepository<StockOut> _repo;

    public GetAllStockOutsHandler(IRepository<StockOut> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockOut>> Handle(GetAllStockOutsQuery request, CancellationToken cancellationToken) => await _repo.GetAllAsync();
}

public record GetAllStockTransfersQuery() : IRequest<IEnumerable<StockTransfer>>;

public class GetAllStockTransfersHandler : IRequestHandler<GetAllStockTransfersQuery, IEnumerable<StockTransfer>>
{
    private readonly IRepository<StockTransfer> _repo;

    public GetAllStockTransfersHandler(IRepository<StockTransfer> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockTransfer>> Handle(GetAllStockTransfersQuery request, CancellationToken cancellationToken) => await _repo.GetAllAsync();
}

public record GetAllStockAuditsQuery() : IRequest<IEnumerable<StockAudit>>;

public class GetAllStockAuditsHandler : IRequestHandler<GetAllStockAuditsQuery, IEnumerable<StockAudit>>
{
    private readonly IRepository<StockAudit> _repo;

    public GetAllStockAuditsHandler(IRepository<StockAudit> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<StockAudit>> Handle(GetAllStockAuditsQuery request, CancellationToken cancellationToken) => await _repo.GetAllAsync();
}
