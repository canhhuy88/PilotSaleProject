using BizI.Application.Seed.Seeders;

namespace BizI.Application.Seed;

/// <summary>
/// Orchestrates all seeders in dependency order:
///   1. RoleSeeder   — creates Admin / Staff roles (User.RoleId FK target)
///   2. UserSeeder   — creates admin / staff users (depends on roles)
///   3. CategorySeeder — creates Electronics / Fashion / Food
///   4. ProductSeeder  — creates products linked to categories
/// Calling SeedAsync() is idempotent; each individual seeder guards against duplicates.
/// </summary>
public sealed class SeedService
{
    private readonly RoleSeeder _roleSeeder;
    private readonly UserSeeder _userSeeder;
    private readonly CategorySeeder _categorySeeder;
    private readonly ProductSeeder _productSeeder;

    public SeedService(
        RoleSeeder roleSeeder,
        UserSeeder userSeeder,
        CategorySeeder categorySeeder,
        ProductSeeder productSeeder)
    {
        _roleSeeder = roleSeeder;
        _userSeeder = userSeeder;
        _categorySeeder = categorySeeder;
        _productSeeder = productSeeder;
    }

    public async Task SeedAsync()
    {
        await _roleSeeder.SeedAsync();
        await _userSeeder.SeedAsync();
        await _categorySeeder.SeedAsync();
        await _productSeeder.SeedAsync();
    }
}
