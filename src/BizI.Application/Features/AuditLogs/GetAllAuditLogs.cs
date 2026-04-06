using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.AuditLogs;

public record GetAllAuditLogsQuery() : IRequest<IEnumerable<AuditLog>>;

public class GetAllAuditLogsHandler : IRequestHandler<GetAllAuditLogsQuery, IEnumerable<AuditLog>>
{
    private readonly IRepository<AuditLog> _repo;

    public GetAllAuditLogsHandler(IRepository<AuditLog> repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<AuditLog>> Handle(GetAllAuditLogsQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetAllAsync();
    }
}
