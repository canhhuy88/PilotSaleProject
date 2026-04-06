using MediatR;
using Microsoft.Extensions.Logging;
using BizI.Domain.Entities;
using BizI.Domain.Interfaces;
using BizI.Application.Common;

namespace BizI.Application.Features.Categories;

public record DeleteCategoryCommand(string Id) : IRequest<CommandResult>;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CommandResult>
{
    private readonly IRepository<Category> _categoryRepo;
    private readonly ILogger<DeleteCategoryHandler> _logger;

    public DeleteCategoryHandler(IRepository<Category> categoryRepo, ILogger<DeleteCategoryHandler> logger)
    {
        _categoryRepo = categoryRepo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteCategoryCommand for Id: {CategoryId}", request.Id);

        try
        {
            await _categoryRepo.DeleteAsync(request.Id);
            _logger.LogInformation("Category deleted successfully with Id: {CategoryId}", request.Id);

            return CommandResult.SuccessResult(request.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete category: {CategoryId}", request.Id);
            throw;
        }
    }
}
