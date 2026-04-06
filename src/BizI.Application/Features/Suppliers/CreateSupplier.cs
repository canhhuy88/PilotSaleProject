using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Suppliers;

public record CreateSupplierCommand(string Name, string Phone, string Address) : IRequest<CommandResult>;

public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateSupplierHandler : IRequestHandler<CreateSupplierCommand, CommandResult>
{
    private readonly IRepository<Supplier> _repo;
    private readonly ILogger<CreateSupplierHandler> _logger;

    public CreateSupplierHandler(IRepository<Supplier> repo, ILogger<CreateSupplierHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateSupplierCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Supplier {Name}", request.Name);
        var entity = new Supplier { Name = request.Name, Phone = request.Phone, Address = request.Address };
        await _repo.AddAsync(entity);
        return CommandResult.SuccessResult(entity.Id);
    }
}
