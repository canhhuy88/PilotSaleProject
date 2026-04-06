using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Users;

public record DeleteUserCommand(string Id) : IRequest<CommandResult>;

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, CommandResult>
{
    private readonly IRepository<User> _repo;
    private readonly ILogger<DeleteUserHandler> _logger;

    public DeleteUserHandler(IRepository<User> repo, ILogger<DeleteUserHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting User {Id}", request.Id);
        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
