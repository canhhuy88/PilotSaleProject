using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Categories;

public record UpdateCategoryCommand(string Id, string Name, string? ParentId, string? Description) : IRequest<CommandResult>;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CommandResult>
{
    private readonly IRepository<Category> _categoryRepo;
    private readonly ILogger<UpdateCategoryHandler> _logger;

    public UpdateCategoryHandler(IRepository<Category> categoryRepo, ILogger<UpdateCategoryHandler> logger)
    {
        _categoryRepo = categoryRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateCategoryCommand for Id: {CategoryId}", request.Id);

        try
        {
            var category = await _categoryRepo.GetByIdAsync(request.Id);
            if (category == null)
            {
                return CommandResult.FailureResult("Category not found");
            }

            category.Name = request.Name;
            category.ParentId = request.ParentId;
            category.Description = request.Description;

            await _categoryRepo.UpdateAsync(category);

            _logger.LogInformation("Category updated successfully with Id: {CategoryId}", category.Id);

            return CommandResult.SuccessResult(category.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update category: {CategoryId}", request.Id);
            throw;
        }
    }
}
