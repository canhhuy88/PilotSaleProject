using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Seed.Seeders;

/// <summary>
/// Seeds default product categories (Electronics, Fashion, Food).
/// Idempotent — skips any category whose name already exists.
/// </summary>
public sealed class CategorySeeder
{
    private readonly IRepository<Category> _categoryRepo;

    private static readonly string[] DefaultCategories =
    {
        "Electronics",
        "Fashion",
        "Food"
    };

    public CategorySeeder(IRepository<Category> categoryRepo)
    {
        _categoryRepo = categoryRepo;
    }

    public async Task SeedAsync()
    {
        foreach (var name in DefaultCategories)
            await EnsureCategoryAsync(name);
    }

    private async Task EnsureCategoryAsync(string name)
    {
        var existing = await _categoryRepo.FindAsync(c => c.Name == name);
        if (existing.Any()) return;

        var category = Category.Create(name);
        await _categoryRepo.AddAsync(category);
    }
}
