using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Suppliers;

public record UpdateSupplierCommand(string Id, string Name, string Phone, string Address) : IRequest<CommandResult>;

public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateSupplierHandler : IRequestHandler<UpdateSupplierCommand, CommandResult>
{
    private readonly IRepository<Supplier> _repo;
    private readonly ILogger<UpdateSupplierHandler> _logger;

    public UpdateSupplierHandler(IRepository<Supplier> repo, ILogger<UpdateSupplierHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateSupplierCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating Supplier {Id}", request.Id);
        var entity = await _repo.GetByIdAsync(request.Id);
        if (entity == null) return CommandResult.FailureResult("Not found");

        entity.Name = request.Name;
        entity.Phone = request.Phone;
        entity.Address = request.Address;

        await _repo.UpdateAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
