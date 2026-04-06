using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Categories.Create;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CommandResult>
{
    private readonly IRepository<Category> _repo;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(IRepository<Category> repo, ILogger<CreateCategoryHandler> logger)
    {
        _repo = repo;
        _logger = logger;
    }

    public async Task<CommandResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // ✅ Use Domain factory
            var category = Category.Create(request.Name, request.ParentId, request.Description);
            await _repo.AddAsync(category);
            _logger.LogInformation("Category created. Id: {Id}", category.Id);
            return CommandResult.SuccessResult(category.Id);
        }
        catch (DomainException ex)
        {
            return CommandResult.FailureResult(ex.Message);
        }
    }
}
