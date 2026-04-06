using MediatR;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Features.AuditLogs;

public record GetAuditLogByIdQuery(string Id) : IRequest<AuditLog?>;

public class GetAuditLogByIdHandler : IRequestHandler<GetAuditLogByIdQuery, AuditLog?>
{
    private readonly IRepository<AuditLog> _repo;

    public GetAuditLogByIdHandler(IRepository<AuditLog> repo)
    {
        _repo = repo;
    }

    public async Task<AuditLog?> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repo.GetByIdAsync(request.Id);
    }
}
