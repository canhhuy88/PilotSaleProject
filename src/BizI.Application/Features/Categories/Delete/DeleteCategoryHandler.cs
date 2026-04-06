namespace BizI.Application.Features.Categories.Delete;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, CommandResult>
{
    private readonly IRepository<Category> _repo;

    public DeleteCategoryHandler(IRepository<Category> repo) => _repo = repo;

    public async Task<CommandResult> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetByIdAsync(request.Id);
        if (category is null)
            return CommandResult.FailureResult($"Category '{request.Id}' not found.");

        await _repo.DeleteAsync(request.Id);
        return CommandResult.SuccessResult(request.Id);
    }
}
