using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Roles;

public record DeleteRoleCommand(string Id) : IRequest<CommandResult>;

public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, CommandResult>
{
    private readonly IRepository<Role> _repo;
    private readonly ILogger<DeleteRoleHandler> _logger;

    public DeleteRoleHandler(IRepository<Role> repo, ILogger<DeleteRoleHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting Role {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
