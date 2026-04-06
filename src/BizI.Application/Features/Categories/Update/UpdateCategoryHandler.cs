namespace BizI.Application.Features.Categories.Update;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CommandResult>
{
    private readonly IRepository<Category> _repo;

    public UpdateCategoryHandler(IRepository<Category> repo) => _repo = repo;

    public async Task<CommandResult> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetByIdAsync(request.Id);
        if (category is null)
            return CommandResult.FailureResult($"Category '{request.Id}' not found.");

        try
        {
            // ✅ Domain methods
            category.Rename(request.Name);
            category.UpdateDescription(request.Description);
            await _repo.UpdateAsync(category);
            return CommandResult.SuccessResult(category.Id);
        }
        catch (DomainException ex) { return CommandResult.FailureResult(ex.Message); }
    }
}
