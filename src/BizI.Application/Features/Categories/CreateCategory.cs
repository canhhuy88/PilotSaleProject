using MediatR;
using FluentValidation;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common; // Assuming CommandResult exists here, let me use the namespace from CreateProduct

namespace BizI.Application.Features.Categories;

public record CreateCategoryCommand(string Name, string? ParentId, string? Description) : IRequest<CommandResult>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CommandResult>
{
    private readonly IRepository<Category> _categoryRepo;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(IRepository<Category> categoryRepo, ILogger<CreateCategoryHandler> logger)
    {
        _categoryRepo = categoryRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateCategoryCommand: {CategoryName}", request.Name);

        try
        {
            var category = new Category
            {
                Name = request.Name,
                ParentId = request.ParentId,
                Description = request.Description
            };
            await _categoryRepo.AddAsync(category);

            _logger.LogInformation("Category created successfully with Id: {CategoryId}", category.Id);

            return CommandResult.SuccessResult(category.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create category: {CategoryName}", request.Name);
            throw;
        }
    }
}
