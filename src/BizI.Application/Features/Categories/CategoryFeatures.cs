using BizI.Application.DTOs.Category;
using Microsoft.Extensions.Logging;

namespace BizI.Application.Features.Categories;

// ── Commands/Queries ─────────────────────────────────────────────────────────

public record CreateCategoryCommand(
    string Name,
    string? ParentId = null,
    string? Description = null) : IRequest<CommandResult>;

public record UpdateCategoryCommand(
    string Id,
    string Name,
    string? Description = null) : IRequest<CommandResult>;

public record DeleteCategoryCommand(string Id) : IRequest<CommandResult>;

public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryDto>>;

public record GetCategoryByIdQuery(string Id) : IRequest<CategoryDto?>;

// ── Validators ───────────────────────────────────────────────────────────────

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}

// ── Handlers ─────────────────────────────────────────────────────────────────

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

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryDto>>
{
    private readonly IRepository<Category> _repo;
    private readonly IMapper _mapper;
    private readonly ICurrentUserService _currentUser;

    public GetAllCategoriesHandler(IRepository<Category> repo, IMapper mapper, ICurrentUserService currentUser  )
    {
        _repo = repo;
        _mapper = mapper;
        _currentUser = currentUser;
    }

    public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    { 
        var userId = _currentUser.Username;
        var all = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(all);
    }
}

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto?>
{
    private readonly IRepository<Category> _repo;
    private readonly IMapper _mapper;

    public GetCategoryByIdHandler(IRepository<Category> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _repo.GetByIdAsync(request.Id);
        return category is null ? null : _mapper.Map<CategoryDto>(category);
    }
}
