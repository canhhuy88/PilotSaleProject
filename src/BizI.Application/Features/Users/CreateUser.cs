using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Users;

public record CreateUserCommand(string Username, string PasswordHash, string FullName, string RoleId, string BranchId, bool IsActive) : IRequest<CommandResult>;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PasswordHash).NotEmpty();
    }
}

public class CreateUserHandler : IRequestHandler<CreateUserCommand, CommandResult>
{
    private readonly IRepository<User> _repo;
    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IRepository<User> repo, ILogger<CreateUserHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating User {Username}", request.Username);
        var entity = new User { Username = request.Username, PasswordHash = request.PasswordHash, FullName = request.FullName, RoleId = request.RoleId, BranchId = request.BranchId, IsActive = request.IsActive };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
