using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Roles;

public record UpdateRoleCommand(string Id, string Name, string[] Permissions) : IRequest<CommandResult>;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, CommandResult>
{
    private readonly IRepository<Role> _repo;
    private readonly ILogger<UpdateRoleHandler> _logger;

    public UpdateRoleHandler(IRepository<Role> repo, ILogger<UpdateRoleHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Role {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.Name = request.Name;
        entity.Permissions = request.Permissions;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
