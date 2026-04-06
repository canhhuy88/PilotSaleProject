using BizI.Domain.Entities;
using BizI.Domain.Interfaces;

namespace BizI.Application.Seed.Seeders;

/// <summary>
/// Seeds sample products, each linked to the matching Category.
/// Idempotent — skips any product whose name already exists.
/// </summary>
public sealed class ProductSeeder
{
    private readonly IRepository<Product> _productRepo;
    private readonly IRepository<Category> _categoryRepo;

    // (ProductName, SKU, CostPrice, SalePrice, Unit, CategoryName)
    private static readonly (string Name, string Sku, decimal Cost, decimal Sale, string Unit, string Category)[] SeedData =
    {
        ("iPhone",   "IPHONE-001",  15_000_000m, 20_000_000m, "pcs",  "Electronics"),
        ("T-Shirt",  "TSHIRT-001",      80_000m,    150_000m, "pcs",  "Fashion"),
        ("Milk",     "MILK-001",         15_000m,     25_000m, "box",  "Food"),
    };

    public ProductSeeder(IRepository<Product> productRepo, IRepository<Category> categoryRepo)
    {
        _productRepo = productRepo;
        _categoryRepo = categoryRepo;
    }

    public async Task SeedAsync()
    {
        foreach (var (name, sku, cost, sale, unit, categoryName) in SeedData)
            await EnsureProductAsync(name, sku, cost, sale, unit, categoryName);
    }

    private async Task EnsureProductAsync(
        string name, string sku, decimal cost, decimal sale, string unit, string categoryName)
    {
        var existing = await _productRepo.FindAsync(p => p.Name == name);
        if (existing.Any()) return;

        var categories = await _categoryRepo.FindAsync(c => c.Name == categoryName);
        var category = categories.FirstOrDefault();
        if (category is null) return; // category must exist first

        var product = Product.Create(
            name: name,
            sku: sku,
            costPrice: cost,
            salePrice: sale,
            unit: unit,
            categoryId: category.Id);

        await _productRepo.AddAsync(product);
    }
}
