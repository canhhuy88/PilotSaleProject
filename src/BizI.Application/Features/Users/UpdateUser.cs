using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Users;

public record UpdateUserCommand(string Id, string Username, string PasswordHash, string FullName, string RoleId, string BranchId, bool IsActive) : IRequest<CommandResult>;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Username).NotEmpty().MaximumLength(200);
    }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, CommandResult>
{
    private readonly IRepository<User> _repo;
    private readonly ILogger<UpdateUserHandler> _logger;

    public UpdateUserHandler(IRepository<User> repo, ILogger<UpdateUserHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating User {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.Username = request.Username;
        entity.PasswordHash = request.PasswordHash;
        entity.FullName = request.FullName;
        entity.RoleId = request.RoleId;
        entity.BranchId = request.BranchId;
        entity.IsActive = request.IsActive;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
