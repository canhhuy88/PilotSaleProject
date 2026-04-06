using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Roles;

public record CreateRoleCommand(string Name, string[] Permissions) : IRequest<CommandResult>;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, CommandResult>
{
    private readonly IRepository<Role> _repo;
    private readonly ILogger<CreateRoleHandler> _logger;

    public CreateRoleHandler(IRepository<Role> repo, ILogger<CreateRoleHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Role {Name}", request.Name);
        var entity = new Role { Name = request.Name, Permissions = request.Permissions };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
