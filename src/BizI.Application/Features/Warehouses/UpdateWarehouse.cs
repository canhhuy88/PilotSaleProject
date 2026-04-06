using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Warehouses;

public record UpdateWarehouseCommand(string Id, string Name, string BranchId) : IRequest<CommandResult>;

public class UpdateWarehouseCommandValidator : AbstractValidator<UpdateWarehouseCommand>
{
    public UpdateWarehouseCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateWarehouseHandler : IRequestHandler<UpdateWarehouseCommand, CommandResult>
{
    private readonly IRepository<Warehouse> _repo;
    private readonly ILogger<UpdateWarehouseHandler> _logger;

    public UpdateWarehouseHandler(IRepository<Warehouse> repo, ILogger<UpdateWarehouseHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateWarehouseCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Warehouse {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.Name = request.Name;
        entity.BranchId = request.BranchId;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
